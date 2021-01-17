using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.MoveActivityToCard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Activities;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.MoveActivityToCard
{
    public class MoveActivityToCardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoveActivityToCardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_ActivityIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(MoveActivityToCardCommandHandlerTests)}_{nameof(Handle_ActivityIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card = Card.Create("card-title");
            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new MoveActivityToCardCommandHandler(context);
            var request = new MoveActivityToCardCommand { ActivityId = 1, CardId = 1 };

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_CardIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(MoveActivityToCardCommandHandlerTests)}_{nameof(Handle_CardIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var activity = Activity.Create("activity-title");
            context.Activities.Add(activity);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new MoveActivityToCardCommandHandler(context);
            var request = new MoveActivityToCardCommand { ActivityId = 1, CardId = 1 };

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_ActivityIdAndCardIdExist_MovesActivityToCard()
        {
            var dbName = $"{nameof(MoveActivityToCardCommandHandlerTests)}_{nameof(Handle_ActivityIdAndCardIdExist_MovesActivityToCard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card1 = Card.Create("card1-title");
            var card2 = Card.Create("card2-title");
            var activity = Activity.Create("activity-title");
            card1.AddActivity(activity);
            context.Cards.AddRange(card1, card2);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new MoveActivityToCardCommandHandler(context);
            var request = new MoveActivityToCardCommand { ActivityId = activity.Id, CardId = card2.Id };

            await handler.Handle(request, cancellationToken);

            var activityFromDb = await context.Activities.FirstOrDefaultAsync(a => a.Id == activity.Id, cancellationToken);

            Assert.NotNull(activityFromDb);
            Assert.Equal(card2.Id, activityFromDb.CardId);
        }

        [Fact]
        public async Task Handle_ActivityIdAndCardIdExist_SetsActivityOrderToNull()
        {
            var dbName = $"{nameof(MoveActivityToCardCommandHandlerTests)}_{nameof(Handle_ActivityIdAndCardIdExist_SetsActivityOrderToNull)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card1 = Card.Create("card1-title");
            var card2 = Card.Create("card2-title");
            var activity = Activity.Create("activity-title", 1);
            card1.AddActivity(activity);
            context.Cards.AddRange(card1, card2);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new MoveActivityToCardCommandHandler(context);
            var request = new MoveActivityToCardCommand { ActivityId = activity.Id, CardId = card2.Id };

            await handler.Handle(request, cancellationToken);

            var activityFromDb = await context.Activities.FirstOrDefaultAsync(a => a.Id == activity.Id, cancellationToken);

            Assert.Null(activityFromDb.Order);
        }
    }
}
