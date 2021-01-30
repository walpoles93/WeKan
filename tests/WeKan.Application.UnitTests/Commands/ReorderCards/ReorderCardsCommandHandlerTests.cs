using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.ReorderCards;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.ReorderCards
{
    public class ReorderCardsCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReorderCardsCommandHandler(null, new CardService()));
        }

        [Fact]
        public void Ctor_ICardServiceNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(ReorderCardsCommandHandlerTests)}_{nameof(Ctor_ICardServiceNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new ReorderCardsCommandHandler(context, null));
        }

        [Fact]
        public async Task Handle_AnyCardIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(ReorderCardsCommandHandlerTests)}_{nameof(Handle_AnyCardIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var card = Card.Create("card-title");
            context.Cards.Add(card);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new ReorderCardsCommandHandler(context, new CardService());
            var request = new ReorderCardsCommand { CardIds = new List<int> { 1, 2 } };

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_AnyCardNotBelongsToSameBoard_ThrowsInvalidOperationApplicationException()
        {
            var dbName = $"{nameof(ReorderCardsCommandHandlerTests)}_{nameof(Handle_AnyCardNotBelongsToSameBoard_ThrowsInvalidOperationApplicationException)}";
            var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var boardFactory = new BoardFactory();
            var board1 = boardFactory.Create("board1-title");
            var board2 = boardFactory.Create("board2-title");
            var card1 = Card.Create("card1-title");
            var card2 = Card.Create("card2-title");
            board1.AddCard(card1);
            board2.AddCard(card2);
            context.Boards.AddRange(board1, board2);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new ReorderCardsCommandHandler(context, new CardService());
            var request = new ReorderCardsCommand { CardIds = new List<int> { 1, 2 } };

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<InvalidOperationApplicationException>(action);
        }

        [Fact]
        public async Task Handle_AllCardsExistAndBelongToSameBoard_ReordersCards()
        {
            var dbName = $"{nameof(ReorderCardsCommandHandlerTests)}_{nameof(Handle_AllCardsExistAndBelongToSameBoard_ReordersCards)}";
            var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var card1 = Card.Create("card1-title");
            var card2 = Card.Create("card2-title");
            board.AddCard(card1);
            board.AddCard(card2);
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new ReorderCardsCommandHandler(context, new CardService());
            var request = new ReorderCardsCommand { CardIds = new List<int> { 2, 1 } };

            await handler.Handle(request, cancellationToken);

            var card1FromDb = await context.Cards.FirstOrDefaultAsync(c => c.Id == 1, cancellationToken);
            var card2FromDb = await context.Cards.FirstOrDefaultAsync(c => c.Id == 2, cancellationToken);

            Assert.NotNull(card1FromDb);
            Assert.NotNull(card2FromDb);
            Assert.Equal(0, card2FromDb.Order);
            Assert.Equal(1, card1FromDb.Order);
        }
    }
}
