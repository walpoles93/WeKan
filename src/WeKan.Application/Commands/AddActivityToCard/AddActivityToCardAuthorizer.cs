using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.AddActivityToCard
{
    public class AddActivityToCardAuthorizer : IAuthorizer<AddActivityToCardCommand>
    {
        private readonly ICurrentUserPermissionService _currentUserPermissionService;

        public AddActivityToCardAuthorizer(ICurrentUserPermissionService currentUserPermissionService)
        {
            _currentUserPermissionService = currentUserPermissionService ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public async Task<bool> Authorise(AddActivityToCardCommand request, CancellationToken cancellationToken = default)
        {
            return await _currentUserPermissionService.HasPermissionForCard(request.CardId, BoardUserPermission.CAN_CREATE_ACTIVITY, cancellationToken);
        }
    }
}
