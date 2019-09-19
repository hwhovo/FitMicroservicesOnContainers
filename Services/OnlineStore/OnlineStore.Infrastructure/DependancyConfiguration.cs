using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.BLL.Operations;
using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.DAL;
using OnlineStore.DAL.Repositories;

namespace OnlineStore.Infrastructure
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
            {typeof(IProductRepository), typeof(ProductRepository)},
            {typeof(IOrderRepository), typeof(OrderRepository)},
            {typeof(IOrderProductRepository), typeof(OrderProductRepository)},
            {typeof(ICategoryRepository), typeof(CategoryRepository)},
        };

        private static readonly Dictionary<Type, Type> BussinesLayerServiceAndImplementationTypes = new Dictionary<Type, Type>
        {
            {typeof(IUserOperations),typeof(UserOperations)},
            {typeof(IOrderOperations),typeof(OrderOperations)},
            {typeof(IProductOperations),typeof(ProductOperations)},
            {typeof(ICategoryOperations),typeof(CategoryOperations)},
        };
    }
}
