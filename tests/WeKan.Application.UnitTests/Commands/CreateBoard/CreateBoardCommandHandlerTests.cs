using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.CreateBoard;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.CreateBoard
{
    public class CreateBoardCommandHandlerTests
    {
        private readonly Mock<ICurrentUser> _currentUser;

        public CreateBoardCommandHandlerTests()
        {
            _currentUser = new Mock<ICurrentUser>();
        }

        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateBoardCommandHandler(null, _currentUser.Object, new BoardFactory(), new BoardUserFactory()));
        }

        [Fact]
        public void Ctor_IBoardUserFactoryNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Ctor_IBoardUserFactoryNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new CreateBoardCommandHandler(context, _currentUser.Object, new BoardFactory(), null));
        }

        [Fact]
        public void Ctor_ICurrentUserNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Ctor_ICurrentUserNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new CreateBoardCommandHandler(context, null, new BoardFactory(), new BoardUserFactory()));
        }

        [Fact]
        public void Ctor_IBoardFactory_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Ctor_IBoardFactory_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new CreateBoardCommandHandler(context, _currentUser.Object, null, new BoardUserFactory()));
        }

        [Fact]
        public async Task Handle_CreatesBoard()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Handle_CreatesBoard)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new CreateBoardCommandHandler(context, _currentUser.Object, new BoardFactory(), new BoardUserFactory());
            var request = new CreateBoardCommand { Title = "test-title" };
            var cancellationToken = new CancellationToken();

            await handler.Handle(request, cancellationToken);

            var board = await context.Boards.FirstOrDefaultAsync(cancellationToken);

            Assert.NotNull(board);
            Assert.Equal(1, board.Id);
        }

        [Fact]
        public async Task Handle_CreatesBoardUser()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Handle_CreatesBoardUser)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);
            var handler = new CreateBoardCommandHandler(context, _currentUser.Object, new BoardFactory(), new BoardUserFactory());
            var request = new CreateBoardCommand { Title = "test-title" };
            var cancellationToken = new CancellationToken();

            await handler.Handle(request, cancellationToken);

            var boardUser = await context.BoardUsers.FirstOrDefaultAsync(cancellationToken);

            Assert.NotNull(boardUser);
            Assert.Equal(1, boardUser.Id);
            Assert.Equal(userId, boardUser.UserId);
            Assert.Equal(1, boardUser.BoardId);
        }

        [Fact]
        public async Task Handle_ReturnsBoardCreatedDtoWithCorrectBoardId()
        {
            var dbName = $"{nameof(CreateBoardCommandHandlerTests)}_{nameof(Handle_ReturnsBoardCreatedDtoWithCorrectBoardId)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new CreateBoardCommandHandler(context, _currentUser.Object, new BoardFactory(), new BoardUserFactory());
            var request = new CreateBoardCommand { Title = "test-title" };
            var cancellationToken = new CancellationToken();

            var boardCreatedDto = await handler.Handle(request, cancellationToken);
            var board = await context.Boards.FirstOrDefaultAsync(cancellationToken);

            Assert.NotNull(boardCreatedDto);
            Assert.Equal(board.Id, boardCreatedDto.BoardId);
        }
    }
}
