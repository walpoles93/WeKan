using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.DeleteActivity
{
    public class DeleteActivityAuthorizer : IAuthorizer<DeleteActivityCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public DeleteActivityAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(DeleteActivityCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForActivity(request.ActivityId, BoardUserPermission.CAN_DELETE_ACTIVITY, cancellationToken);
        }
    }
}
