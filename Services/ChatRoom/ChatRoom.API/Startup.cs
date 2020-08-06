using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChatRoom.API.ErrorHandling;
using ChatRoom.API.Middleware;
using ChatRoom.Core.Enums;
using ChatRoom.DAL;
using ChatRoom.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ChatRoom.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers();
            services.AddCors();

            services.AddDbContextPool<ChatRoomContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(StartupConfigurations.DefaultConnection.ToString()), m => m.MigrationsAssembly(GetType().Namespace));
                options.EnableSensitiveDataLogging(true);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo());
            });

            services.AddRepositoriesAndBussinesLayerServices();
            services.AddMvc();
            services.AddJWTAuthentification(Configuration);
            services.AddEventBus(Configuration);
            services.AddEventBusServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "My API V1");
            });
            app.SeedData();
            app.UseWebSockets();
            app.UseEventBusEvents();
            app.UseMiddleware<WebSocketMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
