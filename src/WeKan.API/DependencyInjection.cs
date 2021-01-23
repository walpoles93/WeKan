using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WeKan.API.Services;
using WeKan.Application.Common.Interfaces;

namespace WeKan.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor()
                .AddTransient<ICurrentUser, CurrentUser>()
                .AddSwagger();
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeKan.API", Version = "v1" });
            });
        }
    }
}
