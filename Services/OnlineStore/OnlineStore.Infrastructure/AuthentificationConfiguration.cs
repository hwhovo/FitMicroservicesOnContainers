using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.Operations;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Models;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Infrastructure
{
    public static class AuthentificationConfiguration
    {
        public static void AddJWTAuthentification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserOperations, UserOperations>();

            var authConfigurationSection = configuration.GetSection(StartupConfigurations.TokenAuthentification.ToString());
            services.Configure<TokenAuthentification>(authConfigurationSection);

            var authConfiguration = authConfigurationSection.Get<TokenAuthentification>();
            var key = Encoding.ASCII.GetBytes(authConfiguration.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserOperations>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            context.Fail(HttpStatusCode.Unauthorized.ToString());
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
