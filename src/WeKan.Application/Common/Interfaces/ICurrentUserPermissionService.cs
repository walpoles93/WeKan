using System.Threading;
using System.Threading.Tasks;
using WeKan.Domain.Boards;

namespace WeKan.Application.Common.Interfaces
{
    public interface ICurrentUserPermissionService
    {
        Task<bool> HasPermissionForBoard(int boardId, BoardUserPermission permission, CancellationToken cancellationToken = default);
        Task<bool> HasPermissionForCard(int cardId, BoardUserPermission permission, CancellationToken cancellationToken = default);
        Task<bool> HasPermissionForActivity(int activityId, BoardUserPermission permission, CancellationToken cancellationToken = default);
    }
}
