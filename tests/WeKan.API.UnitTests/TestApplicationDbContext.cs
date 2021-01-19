using Microsoft.EntityFrameworkCore;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;

namespace WeKan.API.UnitTests
{
    public class TestApplicationDbContext : DbContext, IApplicationDbContext
    {
        public static TestApplicationDbContext Create(string name)
        {
            var builder = new DbContextOptionsBuilder<TestApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: name);

            var context = new TestApplicationDbContext(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public TestApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
