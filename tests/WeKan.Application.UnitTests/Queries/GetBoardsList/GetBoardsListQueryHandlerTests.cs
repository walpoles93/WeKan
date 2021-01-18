using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Queries.GetBoardsList;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Queries.GetBoardsList
{
    public class GetBoardsListQueryHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetBoardsListQueryHandler(null));
        }

        [Fact]
        public async Task Handle_NoBoards_ReturnsEmptyList()
        {
            var dbName = $"{nameof(GetBoardsListQueryHandlerTests)}_{nameof(Handle_NoBoards_ReturnsEmptyList)}";
            var context = TestApplicationDbContext.Create(dbName);
            var handler = new GetBoardsListQueryHandler(context);

            var dto = await handler.Handle(new GetBoardsListQuery(), new CancellationToken());

            Assert.NotNull(dto.Boards);
            Assert.Empty(dto.Boards);
        }

        [Fact]
        public async Task Handle_HasBoards_ReturnsDto()
        {
            var dbName = $"{nameof(GetBoardsListQueryHandlerTests)}_{nameof(Handle_HasBoards_ReturnsDto)}";
            var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var board = Board.Create("board-title");
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new GetBoardsListQueryHandler(context);

            var dto = await handler.Handle(new GetBoardsListQuery(), cancellationToken);

            Assert.NotNull(dto.Boards);
            Assert.Single(dto.Boards);
        }
    }
}
