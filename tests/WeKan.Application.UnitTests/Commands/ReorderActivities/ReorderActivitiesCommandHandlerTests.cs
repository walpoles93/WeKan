using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.ReorderActivities;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Activities;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.ReorderActivities
{
    public class ReorderActivitiesCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReorderActivitiesCommandHandler(null, new ActivityService()));
        }

        [Fact]
        public void Ctor_IActivityServiceNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(ReorderActivitiesCommandHandlerTests)}_{nameof(Ctor_IActivityServiceNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new ReorderActivitiesCommandHandler(context, null));
        }

        [Fact]
        public async Task Handle_AnyActivityIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(ReorderActivitiesCommandHandlerTests)}_{nameof(Handle_AnyActivityIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var activity = Activity.Create("activity-title");
            context.Activities.Add(activity);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new ReorderActivitiesCommandHandler(context, new ActivityService());
            var request = new ReorderActivitiesCommand { ActivityIds = new List<int> { 1, 2 } };

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_AnyActivityNotBelongsToSameCard_ThrowsInvalidOperationApplicationException()
        {
            var dbName = $"{nameof(ReorderActivitiesCommandHandlerTests)}_{nameof(Handle_AnyActivityNotBelongsToSameCard_ThrowsInvalidOperationApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card1 = Card.Create("card1-title");
            var card2 = Card.Create("card2-title");
            var activity1 = Activity.Create("activity1-title");
            var activity2 = Activity.Create("activity2-title");
            card1.AddActivity(activity1);
            card2.AddActivity(activity2);
            context.Cards.AddRange(card1, card2);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new ReorderActivitiesCommandHandler(context, new ActivityService());
            var request = new ReorderActivitiesCommand { ActivityIds = new List<int> { 1, 2 } };

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<InvalidOperationApplicationException>(action);
        }

        [Fact]
        public async Task Handle_AllActivitiesExistAndBelongToSameCard_ReordersActivities()
        {
            var dbName = $"{nameof(ReorderActivitiesCommandHandlerTests)}_{nameof(Handle_AllActivitiesExistAndBelongToSameCard_ReordersActivities)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card = Card.Create("card-title");
            var activity1 = Activity.Create("activity1-title", 0);
            var activity2 = Activity.Create("activity2-title", 1);
            card.AddActivity(activity1);
            card.AddActivity(activity2);
            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new ReorderActivitiesCommandHandler(context, new ActivityService());
            var request = new ReorderActivitiesCommand { ActivityIds = new List<int> { 2, 1 } };

            await handler.Handle(request, cancellationToken);

            var activity1FromDb = await context.Activities.FirstOrDefaultAsync(a => a.Id == 1, cancellationToken);
            var activity2FromDb = await context.Activities.FirstOrDefaultAsync(a => a.Id == 2, cancellationToken);

            Assert.NotNull(activity1FromDb);
            Assert.NotNull(activity2FromDb);
            Assert.Equal(0, activity2FromDb.Order);
            Assert.Equal(1, activity1FromDb.Order);
        }
    }
}
