using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using WeKan.Infrastructure.Persistance;
using Xunit;

namespace WeKan.Infrastructure.UnitTests.Persistance
{
    public class ApplicationDbContextTests
    {
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

            var boardFactory = new BoardFactory();
            var board = boardFactory.Create("board-title");
            context.Boards.Add(board);

            await context.SaveChangesAsync(cancellationToken);

            var boardFromDb = await context.Boards.IgnoreQueryFilters().FirstOrDefaultAsync(cancellationToken);

            Assert.NotNull(boardFromDb);
            Assert.Equal(userId, boardFromDb.CreatedByUserId);

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
