using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.AddCardToBoard
{
    public class AddCardToBoardAuthorizer : IAuthorizer<AddCardToBoardCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public AddCardToBoardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(AddCardToBoardCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForBoard(request.BoardId, BoardUserPermission.CAN_CREATE_CARD, cancellationToken);
        }
    }
}
