using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.JoinBoard;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.JoinBoard
{
    public class JoinBoardCommandHandlerTests
    {
        private readonly Mock<ICurrentUser> _currentUser;

        public JoinBoardCommandHandlerTests()
        {
            _currentUser = new Mock<ICurrentUser>();
        }

        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new JoinBoardCommandHandler(null, _currentUser.Object, new BoardUserFactory()));
        }

        [Fact]
        public void Ctor_ICurrentUserNull_ThrowsNullArgumentException()
        {
            var dbName = $"{nameof(JoinBoardCommandHandlerTests)}_{nameof(Ctor_ICurrentUserNull_ThrowsNullArgumentException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new JoinBoardCommandHandler(context, null, new BoardUserFactory()));
        }

        [Fact]
        public void Ctor_IBoardUserFactory_ThrowsNullArgumentException()
        {
            var dbName = $"{nameof(JoinBoardCommandHandlerTests)}_{nameof(Ctor_IBoardUserFactory_ThrowsNullArgumentException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new JoinBoardCommandHandler(context, _currentUser.Object, null));
        }

        [Fact]
        public async Task Handle_AccessCodeNotFound_ThrowNotFoundApplicationException()
        {
            var dbName = $"{nameof(JoinBoardCommandHandlerTests)}_{nameof(Handle_AccessCodeNotFound_ThrowNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new JoinBoardCommandHandler(context, _currentUser.Object, new BoardUserFactory());
            var command = new JoinBoardCommand { AccessCode = "accessCode" };
            var cancellationToken = new CancellationToken();

            Task<BoardJoinedDto> action() => handler.Handle(command, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_AccessCodeFound_CreateBoardUser()
        {
            var dbName = $"{nameof(JoinBoardCommandHandlerTests)}_{nameof(Handle_AccessCodeFound_CreateBoardUser)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);

            var handler = new JoinBoardCommandHandler(context, _currentUser.Object, new BoardUserFactory());
            var command = new JoinBoardCommand { AccessCode = board.AccessCode };
            var dto = await handler.Handle(command, cancellationToken);

            var boardUser = await context.BoardUsers
                .FirstOrDefaultAsync(bu => bu.BoardId == board.Id && bu.UserId == userId);

            Assert.NotNull(boardUser);
            Assert.Equal(board.Id, dto.BoardId);
        }
    }
}
