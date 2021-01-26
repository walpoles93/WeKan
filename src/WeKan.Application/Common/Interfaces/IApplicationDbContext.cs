using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;

namespace WeKan.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Board> Boards { get; }
        DbSet<Card> Cards { get; }
        DbSet<Activity> Activities { get; }
        DbSet<BoardUser> BoardUsers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
