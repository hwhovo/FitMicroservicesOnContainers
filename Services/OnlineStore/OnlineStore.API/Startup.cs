using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineStore.Core.Enums;
using OnlineStore.DAL;
using Swashbuckle.AspNetCore.Swagger;
using OnlineStore.Infrastructure;
using AutoMapper;
using OnlineStore.API.ErrorHandling;
using Microsoft.OpenApi.Models;

namespace OnlineStore.API
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

            services.AddDbContextPool<OnlineStoreContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(StartupConfigurations.DefaultConnection.ToString()), m => m.MigrationsAssembly(GetType().Namespace));
                options.EnableSensitiveDataLogging(true);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo());
            });

            services.AddAutoMapper(x => 
            {
                x.AddProfile<MappingProfile>();
            });
            services.AddRepositoriesAndBussinesLayerServices();
            services.AddJWTAuthentification(Configuration);
            services.AddEventBus(Configuration);
            services.AddEventBusServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "My API V1");
            });
            app.SeedData();
            app.UseEventBusEvents();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
