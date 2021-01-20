using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using WeKan.Infrastructure.Persistance;
using Xunit;

namespace WeKan.Infrastructure.UnitTests.Persistance
{
    public class ApplicationDbContextTests
    {
        // TODO test that user id is set
        // TODO test that requests are filtered by user id

        private readonly Mock<ICurrentUser> _currentUser;

        public ApplicationDbContextTests()
        {
            _currentUser = new Mock<ICurrentUser>();
        }

        [Fact]
        public void Ctor_ICurrentUserNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder().Options;

            Assert.Throws<ArgumentNullException>(() => new ApplicationDbContext(options, null));
        }

        [Fact]
        public async Task SaveChangesAsync_EntityCreatedByUserIdSetToCurrentUserId()
        {
            var userId = "test-user-id-1";
            _currentUser.Setup(u => u.UserId).Returns(userId);

            var dbName = $"{nameof(ApplicationDbContextTests)}_{nameof(SaveChangesAsync_EntityCreatedByUserIdSetToCurrentUserId)}";
            using var context = CreateContext(dbName, _currentUser.Object);
            var cancellationToken = new CancellationToken();

            var board = Board.Create("board-title");
            context.Boards.Add(board);

            await context.SaveChangesAsync(cancellationToken);

            var boardFromDb = await context.Boards.IgnoreQueryFilters().FirstOrDefaultAsync(cancellationToken);

            Assert.NotNull(boardFromDb);
            Assert.Equal(userId, boardFromDb.CreatedByUserId);

        }

        [Fact]
        public async Task Activities_FiltersByCreatedByUserId()
        {
            var dbName = $"{nameof(ApplicationDbContextTests)}_{nameof(Activities_FiltersByCreatedByUserId)}";
            var cancellationToken = new CancellationToken();

            var user1Id = "user1";
            _currentUser.Setup(u => u.UserId).Returns(user1Id);
            using var context1 = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var activity1 = Activity.Create("activity1-title");
            context1.Activities.Add(activity1);
            await context1.SaveChangesAsync(cancellationToken);

            var user2Id = "user2";
            _currentUser.Setup(u => u.UserId).Returns(user2Id);
            using var context2 = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var activity2 = Activity.Create("activity2-title");
            context2.Activities.Add(activity2);
            await context2.SaveChangesAsync(cancellationToken);

            _currentUser.Setup(u => u.UserId).Returns(user1Id);
            using var context = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var activities = await context.Activities.ToListAsync(cancellationToken);

            Assert.Single(activities);
            Assert.Equal(user1Id, activities.First().CreatedByUserId);
        }

        [Fact]
        public async Task Cards_FiltersByCreatedByUserId()
        {
            var dbName = $"{nameof(ApplicationDbContextTests)}_{nameof(Cards_FiltersByCreatedByUserId)}";
            var cancellationToken = new CancellationToken();

            var user1Id = "user1";
            _currentUser.Setup(u => u.UserId).Returns(user1Id);
            using var context1 = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var card1 = Card.Create("card1-title");
            context1.Cards.Add(card1);
            await context1.SaveChangesAsync(cancellationToken);

            var user2Id = "user2";
            _currentUser.Setup(u => u.UserId).Returns(user2Id);
            using var context2 = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var card2 = Card.Create("card2-title");
            context2.Cards.Add(card2);
            await context2.SaveChangesAsync(cancellationToken);

            _currentUser.Setup(u => u.UserId).Returns(user1Id);
            using var context = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var cards = await context.Cards.ToListAsync(cancellationToken);

            Assert.Single(cards);
            Assert.Equal(user1Id, cards.First().CreatedByUserId);
        }

        [Fact]
        public async Task Boards_FiltersByCreatedByUserId()
        {
            var dbName = $"{nameof(ApplicationDbContextTests)}_{nameof(Boards_FiltersByCreatedByUserId)}";
            var cancellationToken = new CancellationToken();

            var user1Id = "user1";
            _currentUser.Setup(u => u.UserId).Returns(user1Id);
            using var context1 = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var board1 = Board.Create("baord1-title");
            context1.Boards.Add(board1);
            await context1.SaveChangesAsync(cancellationToken);

            var user2Id = "user2";
            _currentUser.Setup(u => u.UserId).Returns(user2Id);
            using var context2 = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var board2 = Board.Create("board2-title");
            context2.Boards.Add(board2);
            await context2.SaveChangesAsync(cancellationToken);

            _currentUser.Setup(u => u.UserId).Returns(user1Id);
            using var context = CreateContext(dbName, _currentUser.Object, ensureDeleted: false);
            var boards = await context.Boards.ToListAsync(cancellationToken);

            Assert.Single(boards);
            Assert.Equal(user1Id, boards.First().CreatedByUserId);
        }

        private ApplicationDbContext CreateContext(string name, ICurrentUser currentUser, bool ensureDeleted = true)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: name);

            var context = new ApplicationDbContext(builder.Options, currentUser);

            if (ensureDeleted) context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
