using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WeKan.Application.Commands.AddActivityToCard;
using WeKan.Application.Commands.AddCardToBoard;
using WeKan.Application.Commands.DeleteActivity;
using WeKan.Application.Commands.DeleteBoard;
using WeKan.Application.Commands.DeleteCard;
using WeKan.Application.Commands.EditActivity;
using WeKan.Application.Commands.EditBoard;
using WeKan.Application.Commands.EditCard;
using WeKan.Application.Commands.MoveActivityToCard;
using WeKan.Application.Commands.ReorderActivities;
using WeKan.Application.Commands.ReorderCards;
using WeKan.Application.Common.Behaviours;
using WeKan.Application.Common.Interfaces;
using WeKan.Application.Common.Services;
using WeKan.Application.Queries.GetBoard;

namespace WeKan.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddMediator()
                .AddAuthorizers()
                .AddServices();
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

            return services;
        }

        private static IServiceCollection AddAuthorizers(this IServiceCollection services)
        {
            return services
                .AddTransient<IAuthorizer<AddActivityToCardCommand>, AddActivityToCardAuthorizer>()
                .AddTransient<IAuthorizer<AddCardToBoardCommand>, AddCardToBoardAuthorizer>()
                .AddTransient<IAuthorizer<DeleteActivityCommand>, DeleteActivityAuthorizer>()
                .AddTransient<IAuthorizer<DeleteBoardCommand>, DeleteBoardAuthorizer>()
                .AddTransient<IAuthorizer<DeleteCardCommand>, DeleteCardAuthorizer>()
                .AddTransient<IAuthorizer<EditActivityCommand>, EditActivityAuthorizer>()
                .AddTransient<IAuthorizer<EditBoardCommand>, EditBoardAuthorizer>()
                .AddTransient<IAuthorizer<EditCardCommand>, EditCardAuthorizer>()
                .AddTransient<IAuthorizer<MoveActivityToCardCommand>, MoveActivityToCardAuthorizer>()
                .AddTransient<IAuthorizer<ReorderActivitiesCommand>, ReorderActivitiesAuthorizer>()
                .AddTransient<IAuthorizer<ReorderCardsCommand>, ReorderCardsAuthorizer>()
                .AddTransient<IAuthorizer<GetBoardQuery>, GetBoardAuthorizer>();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ICurrentUserPermissionService, CurrentUserPermissionService>();
        }
    }
}
