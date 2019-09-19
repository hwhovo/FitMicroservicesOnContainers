using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.API.ErrorHandling;
using OnlineStore.Core.Enums;
using OnlineStore.DAL;
using OnlineStore.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace OnlineStore.API
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

            services.AddDbContextPool<OnlineStoreContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(StartupConfigurations.DefaultConnection.ToString()), m => m.MigrationsAssembly(GetType().Namespace));
                options.EnableSensitiveDataLogging(true);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = GetType().Namespace });
            });

            services.AddAutoMapper();
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
            app.UseEventBusEvents();
            app.UseMvc();
        }
    }
}
