using Autofac;
using EventBus.Core.Abstractions;
using EventBus.Core.Managers;
using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineStore.BLL.IntegrationEventHandlers;
using OnlineStore.Core.Enums;
using OnlineStore.Core.ExceptionTypes;
using OnlineStore.Core.IntegrationEvents.Events;
using OnlineStore.Core.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace OnlineStore.Infrastructure
{
    public static class EventBusConfiguration
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusConfigurationSection = configuration.GetSection(StartupConfigurations.EventBusConfiguration.ToString());
            services.Configure<EventBusConfigurationModel>(eventBusConfigurationSection);

            var eventBusConfiguration = eventBusConfigurationSection.Get<EventBusConfigurationModel>();
            var retryCount = eventBusConfiguration.RetryCount ?? 5;

            
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                logger.LogError(eventBusConfiguration.HostName);
                logger.LogError(eventBusConfiguration.UserName);
                logger.LogError(eventBusConfiguration.Password);
                logger.LogError(eventBusConfiguration.VirtualHost);
                logger.LogError("IP: " + GetLocalIPAddress());

                var factory = new ConnectionFactory()
                {
                    HostName = eventBusConfiguration.HostName ?? throw new LogicException(ExceptionMessage.MANDATORY_PROPERTY_IS_NULL, nameof(eventBusConfiguration.HostName)),
                    // DispatchConsumersAsync = true,
                    VirtualHost = eventBusConfiguration.VirtualHost ?? throw new LogicException(ExceptionMessage.MANDATORY_PROPERTY_IS_NULL, nameof(eventBusConfiguration.HostName)),
                    UserName = eventBusConfiguration.UserName,
                    Password = eventBusConfiguration.Password,
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount, eventBusConfiguration.ConnectionName);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, eventBusConfiguration.SubscriptionQueueName, retryCount);
            });
        }

        public static void AddEventBusServices(this IServiceCollection services)
        {
            services.AddTransient<AddUserIntegrationEventHandler>();
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }

        public static void UseEventBusEvents(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<AddUserIntegrationEvent, AddUserIntegrationEventHandler>();
        }
    }
}
