using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.EditCard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.EditCard
{
    public class EditCardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditCardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_CardIdNotExists_ThrowsNotFouncApplicationException()
        {
            var dbName = $"{nameof(EditCardCommandHandlerTests)}_{nameof(Handle_CardIdNotExists_ThrowsNotFouncApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new EditCardCommandHandler(context);
            var request = new EditCardCommand { CardId = 1, Title = "test-title" };
            var cancellationToken = new CancellationToken();

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_CardIdExists_EditsCard()
        {
            var dbName = $"{nameof(EditCardCommandHandlerTests)}_{nameof(Handle_CardIdExists_EditsCard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card = Card.Create("test-title");
            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var editedTitle = "edited-title";
            var handler = new EditCardCommandHandler(context);
            var request = new EditCardCommand { CardId = 1, Title = editedTitle };

            await handler.Handle(request, cancellationToken);

            var cardFromDb = await context.Cards.FirstOrDefaultAsync(c => c.Id == 1, cancellationToken);

            Assert.NotNull(cardFromDb);
            Assert.Equal(editedTitle, cardFromDb.Title);
        }
    }
}
