using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.AddCardToBoard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.AddCardToBoard
{
    public class AddCardToBoardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AddCardToBoardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_BoardExists_AddsActivityToCard()
        {
            var dbName = $"{nameof(AddCardToBoardCommandHandlerTests)}_{nameof(Handle_BoardExists_AddsActivityToCard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var cancellationToken = new CancellationToken();

            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new AddCardToBoardCommandHandler(context);
            var request = new AddCardToBoardCommand { BoardId = board.Id, Title = "card-title" };
            await handler.Handle(request, cancellationToken);

            var card = await context.Cards.FirstOrDefaultAsync(c => c.BoardId == board.Id);

            Assert.NotNull(card);
            Assert.Equal(1, card.Id);
            Assert.Equal(board.Id, card.BoardId);
        }

        [Fact]
        public async Task Handle_CardNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(AddCardToBoardCommandHandlerTests)}_{nameof(Handle_CardNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var boardId = 1;
            var handler = new AddCardToBoardCommandHandler(context);
            var request = new AddCardToBoardCommand { BoardId = boardId, Title = "card-title" };
            var cancellationToken = new CancellationToken();

            Task<CardCreatedDto> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_BoardExists_ReturnsCardCreatedDtoWithCorrectPropValues()
        {
            var dbName = $"{nameof(AddCardToBoardCommandHandlerTests)}_{nameof(Handle_BoardExists_ReturnsCardCreatedDtoWithCorrectPropValues)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var cancellationToken = new CancellationToken();

            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new AddCardToBoardCommandHandler(context);
            var request = new AddCardToBoardCommand { BoardId = board.Id, Title = "card-title" };

            var cardCreatedDto = await handler.Handle(request, cancellationToken);
            var card = await context.Cards.FirstOrDefaultAsync(c => c.BoardId == board.Id);

            Assert.NotNull(cardCreatedDto);
            Assert.Equal(card.Id, cardCreatedDto.CardId);
            Assert.Equal(card.BoardId, cardCreatedDto.BoardId);
        }
    }
}
