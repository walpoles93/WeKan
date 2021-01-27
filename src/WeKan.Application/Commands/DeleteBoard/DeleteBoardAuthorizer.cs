using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.DeleteBoard
{
    public class DeleteBoardAuthorizer : IAuthorizer<DeleteBoardCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public DeleteBoardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(DeleteBoardCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForBoard(request.BoardId, BoardUserPermission.CAN_DELETE_BOARD, cancellationToken);
        }
    }
}
