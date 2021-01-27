using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Queries.GetBoard
{
    public class GetBoardAuthorizer : IAuthorizer<GetBoardQuery>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public GetBoardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(GetBoardQuery request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForBoard(request.BoardId, BoardUserPermission.CAN_VIEW_BOARD, cancellationToken);
        }
    }
}
