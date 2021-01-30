using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Application.Queries.GetBoardsList;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Queries.GetBoardsList
{
    public class GetBoardsListQueryHandlerTests
    {
        private readonly Mock<ICurrentUser> _currentUser;

        public GetBoardsListQueryHandlerTests()
        {
            _currentUser = new Mock<ICurrentUser>();
        }

        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetBoardsListQueryHandler(null, _currentUser.Object));
        }

        [Fact]
        public void Ctor_ICurrentUserNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(GetBoardsListQueryHandlerTests)}_{nameof(Ctor_ICurrentUserNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new GetBoardsListQueryHandler(context, null));
        }

        [Fact]
        public async Task Handle_NoBoards_ReturnsEmptyList()
        {
            var dbName = $"{nameof(GetBoardsListQueryHandlerTests)}_{nameof(Handle_NoBoards_ReturnsEmptyList)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new GetBoardsListQueryHandler(context, _currentUser.Object);

            var dto = await handler.Handle(new GetBoardsListQuery(), new CancellationToken());

            Assert.NotNull(dto.Boards);
            Assert.Empty(dto.Boards);
        }

        [Fact]
        public async Task Handle_HasBoards_FiltersByBoardUser()
        {
            var factory = new BoardUserFactory();
            var dbName = $"{nameof(GetBoardsListQueryHandlerTests)}_{nameof(Handle_HasBoards_FiltersByBoardUser)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var userId = "user-id";

            _currentUser.Setup(u => u.UserId).Returns(userId);

            var boardFactory = new BoardFactory();
            var board1 = boardFactory.Create("board1-title");
            var board2 = boardFactory.Create("board2-title");
            var boardUser = factory.CreateOwner(1, userId);
            context.Boards.AddRange(board1, board2);
            context.BoardUsers.Add(boardUser);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new GetBoardsListQueryHandler(context, _currentUser.Object);
            var dto = await handler.Handle(new GetBoardsListQuery(), cancellationToken);

            Assert.Single(dto.Boards);
            Assert.Equal(boardUser.BoardId, dto.Boards.First().Id);
        }
    }
}
