using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.DeleteCard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.DeleteCard
{
    public class DeleteCardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteCardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_CardIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(DeleteCardCommandHandlerTests)}_{nameof(Handle_CardIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new DeleteCardCommandHandler(context);
            var request = new DeleteCardCommand { CardId = 1 };
            var cancellationToken = new CancellationToken();

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_CardIdExists_DeleteCard()
        {
            var dbName = $"{nameof(DeleteCardCommandHandlerTests)}_{nameof(Handle_CardIdExists_DeleteCard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card = Card.Create("card-title");
            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new DeleteCardCommandHandler(context);
            var request = new DeleteCardCommand { CardId = 1 };
            await handler.Handle(request, cancellationToken);

            var cardFromDb = await context.Cards.FirstOrDefaultAsync(c => c.Id == 1);

            Assert.Null(cardFromDb);
        }
    }
}
