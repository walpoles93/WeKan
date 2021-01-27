using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.EditActivity
{
    public class EditActivityAuthorizer : IAuthorizer<EditActivityCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public EditActivityAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(EditActivityCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForActivity(request.ActivityId, BoardUserPermission.CAN_EDIT_ACTIVITY, cancellationToken);
        }
    }
}
