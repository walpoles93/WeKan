using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.MoveActivityToCard
{
    public class MoveActivityToCardAuthorizer : IAuthorizer<MoveActivityToCardCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public MoveActivityToCardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(MoveActivityToCardCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForCard(request.CardId, BoardUserPermission.CAN_EDIT_CARD, cancellationToken)
                && await _currentUserPermissionService.HasPermissionForActivity(request.ActivityId, BoardUserPermission.CAN_EDIT_ACTIVITY, cancellationToken);
        }
    }
}
