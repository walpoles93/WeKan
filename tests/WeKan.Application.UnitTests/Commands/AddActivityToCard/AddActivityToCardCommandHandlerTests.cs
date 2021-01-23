using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.AddActivityToCard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.AddActivityToCard
{
    public class AddActivityToCardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AddActivityToCardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_CardExists_AddsActivityToCard()
        {
            var dbName = $"{nameof(AddActivityToCardCommandHandlerTests)}_{nameof(Handle_CardExists_AddsActivityToCard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var card = Card.Create("card-title");
            var cancellationToken = new CancellationToken();

            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new AddActivityToCardCommandHandler(context);
            var request = new AddActivityToCardCommand { CardId = card.Id, Title = "activity-title" };
            await handler.Handle(request, cancellationToken);

            var activity = await context.Activities.FirstOrDefaultAsync(a => a.CardId == card.Id);

            Assert.NotNull(activity);
            Assert.Equal(1, activity.Id);
            Assert.Equal(card.Id, activity.CardId);
        }

        [Fact]
        public async Task Handle_CardNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(AddActivityToCardCommandHandlerTests)}_{nameof(Handle_CardNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cardId = 1;
            var handler = new AddActivityToCardCommandHandler(context);
            var request = new AddActivityToCardCommand { CardId = cardId, Title = "activity-title" };
            var cancellationToken = new CancellationToken();

            Task<ActivityCreatedDto> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_CardExists_ReturnsActivityCreatedDtoWithCorrectPropValues()
        {
            var dbName = $"{nameof(AddActivityToCardCommandHandlerTests)}_{nameof(Handle_CardExists_ReturnsActivityCreatedDtoWithCorrectPropValues)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var card = Card.Create("card-title");
            var cancellationToken = new CancellationToken();

            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new AddActivityToCardCommandHandler(context);
            var request = new AddActivityToCardCommand { CardId = card.Id, Title = "activity-title" };

            var activityCreatedDto = await handler.Handle(request, cancellationToken);
            var activity = await context.Activities.FirstOrDefaultAsync(a => a.CardId == card.Id);

            Assert.NotNull(activityCreatedDto);
            Assert.Equal(activity.Id, activityCreatedDto.ActivityId);
            Assert.Equal(activity.CardId, activityCreatedDto.CardId);
        }
    }
}
