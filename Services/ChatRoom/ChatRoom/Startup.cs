using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChatRoom.API.ErrorHandling;
using ChatRoom.API.Middleware;
using ChatRoom.BLL.IntegrationEventHandlers;
using ChatRoom.BLL.Operations;
using ChatRoom.Core.Abstractions;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Enums;
using ChatRoom.Core.IntegrationEvents.Events;
using ChatRoom.Core.Models;
using ChatRoom.DAL;
using ChatRoom.DAL.Repositories;
using EventBus.Core.Abstractions;
using EventBus.Core.Managers;
using ChatRoom.Infrastructure;

using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddDbContextPool<ChatRoomContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(StartupConfigurations.DefaultConnection.ToString()), m => m.MigrationsAssembly(GetType().Namespace));
                options.EnableSensitiveDataLogging(true);
            });

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info { Title = GetType().Namespace });
            });

            services.AddRepositoriesAndBussinesLayerServices();
            services.AddMvc();
            services.AddJWTAuthentification(Configuration);
            services.AddEventBus(Configuration);
            services.AddEventBusServices();

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "My API V1");
            });
            app.SeedData();
            app.UseMvc();
            app.UseWebSockets();
            app.UseEventBusEvents();
            app.UseMiddleware<WebSocketMiddleware>();
        }
    }
}