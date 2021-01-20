using Microsoft.Extensions.DependencyInjection;
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
                .AddTransient<ICurrentUser, CurrentUser>();
        }
    }
}
