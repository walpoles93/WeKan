using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.EditBoard;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.EditBoard
{
    public class EditBoardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditBoardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_BoardIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(EditBoardCommandHandlerTests)}_{nameof(Handle_BoardIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new EditBoardCommandHandler(context);
            var request = new EditBoardCommand { BoardId = 1, Title = "test-title" };
            var cancellationToken = new CancellationToken();

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_BoardIdExists_EditsBoard()
        {
            var dbName = $"{nameof(EditBoardCommandHandlerTests)}_{nameof(Handle_BoardIdExists_EditsBoard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var board = Board.Create("test-title");
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var editedTitle = "edited-title";
            var handler = new EditBoardCommandHandler(context);
            var request = new EditBoardCommand { BoardId = 1, Title = editedTitle };

            await handler.Handle(request, cancellationToken);

            var boardFromDb = await context.Boards.FirstOrDefaultAsync(b => b.Id == 1, cancellationToken);

            Assert.NotNull(boardFromDb);
            Assert.Equal(editedTitle, boardFromDb.Title);
        }
    }
}
