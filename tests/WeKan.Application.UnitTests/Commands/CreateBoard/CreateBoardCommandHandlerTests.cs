using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.CreateBoard;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.CreateBoard
{
    public class CreateBoardCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateBoardCommandHandler(null));
        }

        [Fact]
        public async Task Handle_CreatesBoard()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Handle_CreatesBoard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new CreateBoardCommandHandler(context);
            var request = new CreateBoardCommand { Title = "test-title" };
            var cancellationToken = new CancellationToken();

            await handler.Handle(request, cancellationToken);

            var board = await context.Boards.FirstOrDefaultAsync(cancellationToken);

            Assert.NotNull(board);
            Assert.Equal(1, board.Id);
        }
    }
}
