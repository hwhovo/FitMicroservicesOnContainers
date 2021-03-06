﻿using ChatRoom.Core.Abstractions;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.Core.Entites;
using ChatRoom.Core.ExceptionTypes;
using ChatRoom.Core.IntegrationEvents.Events;
using EventBus.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.BLL.Operations
{
    public class UserOperations : IUserOperations
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IEventBus _eventBus;

        public UserOperations(IRepositoryManager repositoryManager, IEventBus eventBus)
        {
            _repositoryManager = repositoryManager;
            _eventBus = eventBus;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _repositoryManager.Users.GetSingleAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repositoryManager.Users.FindAsync();
        }
        public User GetById(int id)
        {
            return _repositoryManager.Users.GetSingle(x => x.Id == id);
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _repositoryManager.Users.GetSingleAsync(x => x.Id == id);
        }
        public async Task<User> CreateAsync(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new LogicException("Password is required");

            if (_repositoryManager.Users.Any(x => x.Username == user.Username))
                throw new LogicException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _repositoryManager.Users.Add(user);
            await _repositoryManager.CompleteAsync();

            _eventBus.Publish(new AddUserIntegrationEvent
            {
                FirstName = user.FirstName,
                Username = user.Username,
                Password = password,
                LastName = user.LastName
            });

            return user;
        }
        public async Task UpdateAsync(User userParam, string password = null)
        {
            var user = await _repositoryManager.Users.GetSingleAsync(x => x.Id == userParam.Id);

            if (user == null)
                throw new LogicException("User not found");

            if (userParam.Username != user.Username)
            {
                if (_repositoryManager.Users.Any(x => x.Username == userParam.Username))
                    throw new LogicException("Username " + userParam.Username + " is already taken");
            }

            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _repositoryManager.Users.Update(user);
            await _repositoryManager.CompleteAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var user = await _repositoryManager.Users.GetSingleAsync(x => x.Id == id);
            if (user != null)
            {
                _repositoryManager.Users.Remove(user);
                await _repositoryManager.CompleteAsync();
            }
        }

        #region -- helper methods --

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        #endregion -- helper methods --
    }
}
