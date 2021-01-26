using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using WeKan.Domain.Common.Models;

namespace WeKan.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUser _currentUser;

        public ApplicationDbContext(DbContextOptions options, ICurrentUser currentUser) : base(options)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Activity> Activities { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var newEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            var currentUserId = _currentUser.UserId;

            foreach (var entry in newEntries)
            {
                var entity = entry.Entity as Entity;

                entry.Property(nameof(Entity.CreatedByUserId)).CurrentValue = currentUserId;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
