using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.DeleteCard
{
    public class DeleteCardAuthorizer : IAuthorizer<DeleteCardCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public DeleteCardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(DeleteCardCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForCard(request.CardId, BoardUserPermission.CAN_DELETE_CARD, cancellationToken);
        }
    }
}
