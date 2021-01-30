using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Application.Common.Services;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Common.Services
{
    public class CurrentUserPermissionServiceTests
    {
        private readonly Mock<ICurrentUser> _currentUser;
        private readonly Mock<IBoardUserPermissionService> _boardUserPermissionService;

        public CurrentUserPermissionServiceTests()
        {
            _currentUser = new Mock<ICurrentUser>();
            _boardUserPermissionService = new Mock<IBoardUserPermissionService>();
        }

        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CurrentUserPermissionService(null, _currentUser.Object, _boardUserPermissionService.Object));
        }

        [Fact]
        public void Ctor_ICurrentUserNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(Ctor_ICurrentUserNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new CurrentUserPermissionService(context, null, _boardUserPermissionService.Object));
        }

        [Fact]
        public void Ctor_IBoardUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(Ctor_IBoardUserPermissionServiceNull_ThrowsArgumentNullException)}";
            using var context = TestApplicationDbContext.Create(dbName);

            Assert.Throws<ArgumentNullException>(() => new CurrentUserPermissionService(context, _currentUser.Object, null));
        }

        [Fact]
        public async Task HasPermissionForActivity_ActivityNotExists_ThrowNotFoundApplicationException()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForActivity_ActivityNotExists_ThrowNotFoundApplicationException)}";
            var context = TestApplicationDbContext.Create(dbName);
            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            Task action() => service.HasPermissionForActivity(1, BoardUserPermission.CAN_VIEW_ACTIVITY, new CancellationToken());

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task HasPermissionForActivity_BoardUserNotExists_ReturnFalse()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForActivity_BoardUserNotExists_ReturnFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var card = Card.Create("card-title");
            var activity = Activity.Create("activity-title");
            board.AddCard(card);
            card.AddActivity(activity);
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);

            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            var result = await service.HasPermissionForActivity(activity.Id, BoardUserPermission.CAN_VIEW_ACTIVITY, cancellationToken);

            Assert.False(result);
        }

        [Fact]
        public async Task HasPermissionForActivity_BoardUserExists_ReturnOutputOfIBoardUserPermissionService()
        {
            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);
            _boardUserPermissionService.Setup(s => s.HasPermission(It.IsAny<BoardUser>(), It.IsAny<BoardUserPermission>())).Returns(true);

            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForActivity_BoardUserExists_ReturnOutputOfIBoardUserPermissionService)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var card = Card.Create("card-title");
            var activity = Activity.Create("activity-title");
            var boardUser = new BoardUserFactory().CreateOwner(1, userId);
            board.AddCard(card);
            card.AddActivity(activity);
            context.Boards.Add(board);
            context.BoardUsers.Add(boardUser);
            await context.SaveChangesAsync(cancellationToken);

            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            var result = await service.HasPermissionForActivity(activity.Id, BoardUserPermission.CAN_VIEW_ACTIVITY, cancellationToken);

            Assert.True(result);
        }

        [Fact]
        public async Task HasPermissionForCard_CardNotExists_ThrowNotFoundApplicationException()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForCard_CardNotExists_ThrowNotFoundApplicationException)}";
            var context = TestApplicationDbContext.Create(dbName);
            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            Task action() => service.HasPermissionForCard(1, BoardUserPermission.CAN_VIEW_CARD, new CancellationToken());

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task HasPermissionForCard_BoardUserNotExists_ReturnFalse()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForCard_BoardUserNotExists_ReturnFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var card = Card.Create("card-title");
            board.AddCard(card);
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);

            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            var result = await service.HasPermissionForCard(card.Id, BoardUserPermission.CAN_VIEW_CARD, cancellationToken);

            Assert.False(result);
        }

        [Fact]
        public async Task HasPermissionForCard_BoardUserExists_ReturnOutputOfIBoardUserPermissionService()
        {
            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);
            _boardUserPermissionService.Setup(s => s.HasPermission(It.IsAny<BoardUser>(), It.IsAny<BoardUserPermission>())).Returns(true);

            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForCard_BoardUserExists_ReturnOutputOfIBoardUserPermissionService)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var card = Card.Create("card-title");
            var boardUser = new BoardUserFactory().CreateOwner(1, userId);
            board.AddCard(card);
            context.Boards.Add(board);
            context.BoardUsers.Add(boardUser);
            await context.SaveChangesAsync(cancellationToken);

            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            var result = await service.HasPermissionForCard(card.Id, BoardUserPermission.CAN_VIEW_CARD, cancellationToken);

            Assert.True(result);
        }

        [Fact]
        public async Task HasPermissionForBoard_BoardNotExists_ThrowNotFoundApplicationException()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForBoard_BoardNotExists_ThrowNotFoundApplicationException)}";
            var context = TestApplicationDbContext.Create(dbName);
            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            Task action() => service.HasPermissionForBoard(1, BoardUserPermission.CAN_VIEW_BOARD, new CancellationToken());

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task HasPermissionForBoard_BoardUserNotExists_ReturnFalse()
        {
            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForBoard_BoardUserNotExists_ReturnFalse)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);

            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            var result = await service.HasPermissionForBoard(board.Id, BoardUserPermission.CAN_VIEW_BOARD, cancellationToken);

            Assert.False(result);
        }

        [Fact]
        public async Task HasPermissionForBoard_BoardUserExists_ReturnOutputOfIBoardUserPermissionService()
        {
            var userId = "user-id";
            _currentUser.Setup(u => u.UserId).Returns(userId);
            _boardUserPermissionService.Setup(s => s.HasPermission(It.IsAny<BoardUser>(), It.IsAny<BoardUserPermission>())).Returns(true);

            var dbName = $"{nameof(CurrentUserPermissionServiceTests)}_{nameof(HasPermissionForBoard_BoardUserExists_ReturnOutputOfIBoardUserPermissionService)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            var boardUser = new BoardUserFactory().CreateOwner(1, userId);
            context.Boards.Add(board);
            context.BoardUsers.Add(boardUser);
            await context.SaveChangesAsync(cancellationToken);

            var service = new CurrentUserPermissionService(context, _currentUser.Object, _boardUserPermissionService.Object);

            var result = await service.HasPermissionForBoard(board.Id, BoardUserPermission.CAN_VIEW_BOARD, cancellationToken);

            Assert.True(result);
        }
    }
}
