using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ChatRoom.BLL.Operations;
using ChatRoom.Core.Abstractions;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.DAL;
using ChatRoom.DAL.Repositories;

namespace ChatRoom.Infrastructure
{
    public static class DependancyConfiguration
    {
        public static void AddRepositoriesAndBussinesLayerServices(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddBussinesLayerServices();
        }

        public static void AddRepositories(this IServiceCollection services)
        {

            foreach (var entry in RepositoryServiceAndImplementationTypes)
            {

                services.Add(new ServiceDescriptor(entry.Key, entry.Value, ServiceLifetime.Transient));
            }
        }

        public static void AddBussinesLayerServices(this IServiceCollection services)
        {

            foreach (var entry in BussinesLayerServiceAndImplementationTypes)
            {

                services.Add(new ServiceDescriptor(entry.Key, entry.Value, ServiceLifetime.Transient));
            }
        }

        private static readonly Dictionary<Type, Type> RepositoryServiceAndImplementationTypes = new Dictionary<Type, Type>
        {
            {typeof(IRepositoryManager), typeof(RepositoryManager)},
            {typeof(IUserRepository), typeof(UserRepository)},
            {typeof(IUserChatRoomRepository), typeof(UserChatRoomRepository)},
            {typeof(IMessageRepository), typeof(MessageRepository)},
        };

        private static readonly Dictionary<Type, Type> BussinesLayerServiceAndImplementationTypes = new Dictionary<Type, Type>
        {
            {typeof(IUserOperations),typeof(UserOperations)},
            {typeof(IUserChatRoomOperations),typeof(UserChatRoomOperations)},
        };
    }
}
