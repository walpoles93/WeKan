using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;

[assembly: InternalsVisibleTo("WeKan.Domain.UnitTests")]
namespace WeKan.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IBoardUserFactory, BoardUserFactory>();
            services.AddTransient<IBoardUserPermissionService, BoardUserPermissionService>();

            return services;
        }
    }
}
