using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.ReorderCards
{
    public class ReorderCardsAuthorizer : IAuthorizer<ReorderCardsCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public ReorderCardsAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(ReorderCardsCommand request, CancellationToken cancellationToken = default)
        {
            if (request.CardIds is null || !request.CardIds.Any()) return true;

            return await _currentUserPermissionService.HasPermissionForCard(
                request.CardIds.First(),
                BoardUserPermission.CAN_EDIT_CARD,
                cancellationToken);
        }
    }
}
