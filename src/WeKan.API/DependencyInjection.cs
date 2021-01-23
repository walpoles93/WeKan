using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WeKan.API.Services;
using WeKan.Application.Common.Interfaces;

namespace WeKan.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddHttpContextAccessor()
                .AddTransient<ICurrentUser, CurrentUser>()
                .AddSwagger()
                .AddAuth(configuration);
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeKan.API", Version = "v1" });
            });
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var authenticationSection = configuration.GetSection("Authentication");

                options.Authority = authenticationSection["Authority"];
                options.Audience = authenticationSection["Audience"];
            });

            return services;
        }
    }
}
