using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using WeKan.Domain.Users;

namespace WeKan.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Board> Boards { get; }
        DbSet<Card> Cards { get; }
        DbSet<Activity> Activities { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
