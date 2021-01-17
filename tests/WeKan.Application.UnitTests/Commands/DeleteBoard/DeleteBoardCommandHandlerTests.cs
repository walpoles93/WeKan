using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.DeleteBoard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.DeleteBoard
{
    public class DeleteBoardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteBoardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_BoardIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(DeleteBoardCommandHandlerTests)}_{nameof(Handle_BoardIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new DeleteBoardCommandHandler(context);
            var request = new DeleteBoardCommand { BoardId = 1 };
            var cancellationToken = new CancellationToken();

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_BoardIdExists_DeletesBoard()
        {
            var dbName = $"{nameof(DeleteBoardCommandHandlerTests)}_{nameof(Handle_BoardIdExists_DeletesBoard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var board = Board.Create("board-title");
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new DeleteBoardCommandHandler(context);
            var request = new DeleteBoardCommand { BoardId = 1 };
            await handler.Handle(request, cancellationToken);

            var boardFromDb = await context.Boards.FirstOrDefaultAsync(b => b.Id == 1);

            Assert.Null(boardFromDb);
        }
    }
}
