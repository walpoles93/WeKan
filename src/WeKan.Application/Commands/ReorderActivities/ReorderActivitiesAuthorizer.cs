using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.ReorderActivities
{
    public class ReorderActivitiesAuthorizer : IAuthorizer<ReorderActivitiesCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public ReorderActivitiesAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(ReorderActivitiesCommand request, CancellationToken cancellationToken = default)
        {
            if (request.ActivityIds is null || !request.ActivityIds.Any()) return true;

            return await _currentUserPermissionService.HasPermissionForActivity(
                request.ActivityIds.First(),
                BoardUserPermission.CAN_EDIT_ACTIVITY,
                cancellationToken);
        }
    }
}
