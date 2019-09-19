using ChatRoom.Core.Abstractions;
using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ChatRoom.DAL
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ChatRoomContext _context;

        public RepositoryManager(IServiceProvider serviceProvider, ChatRoomContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        private IUserRepository _users;
        public IUserRepository Users => _users ?? (_users = _serviceProvider.GetService<IUserRepository>());

        private IUserChatRoomRepository _userChatRooms;
        public IUserChatRoomRepository UserChatRooms => _userChatRooms ?? (_userChatRooms = _serviceProvider.GetService<IUserChatRoomRepository>());

        private IMessageRepository _messages;
        public IMessageRepository Messages => _messages ?? (_messages = _serviceProvider.GetService<IMessageRepository>());

        public IDbTransaction BeginTransaction() => new EntityDbTransaction(_context);
        public int Complete() => _context.SaveChanges();
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
