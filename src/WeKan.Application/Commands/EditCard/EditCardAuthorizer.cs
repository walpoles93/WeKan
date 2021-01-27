using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.EditCard
{
    public class EditCardAuthorizer : IAuthorizer<EditCardCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public EditCardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(EditCardCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForCard(request.CardId, BoardUserPermission.CAN_EDIT_CARD, cancellationToken);
        }
    }
}
