using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Common.Services
{
    public class CurrentUserPermissionService : ICurrentUserPermissionService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardUserPermissionService _boardUserPermissionService;

        public CurrentUserPermissionService(
            IApplicationDbContext dbContext,
            ICurrentUser currentUser,
            IBoardUserPermissionService boardUserPermissionService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _boardUserPermissionService = boardUserPermissionService ?? throw new ArgumentNullException(nameof(boardUserPermissionService));
        }

        public async Task<bool> HasPermissionForActivity(int activityId, BoardUserPermission permission, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.UserId;

            var cardId = await _dbContext.Activities
                .Where(a => a.Id == activityId)
                .Select(a => a.CardId)
                .FirstOrDefaultAsync(cancellationToken);
            if (cardId == default) throw new NotFoundApplicationException($"Could not find activity with ID: {activityId}");

            var boardId = await _dbContext.Cards
                .Where(c => c.Id == cardId)
                .Select(c => c.BoardId)
                .FirstOrDefaultAsync(cancellationToken);

            var boardUser = await _dbContext.BoardUsers.FirstOrDefaultAsync(bu => bu.BoardId == boardId && bu.UserId == currentUserId, cancellationToken);
            if (boardUser is null) return false;

            return _boardUserPermissionService.HasPermission(boardUser, permission);
        }

        public async Task<bool> HasPermissionForBoard(int boardId, BoardUserPermission permission, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.UserId;

            var boardExists = await _dbContext.Boards.Where(b => b.Id == boardId).AnyAsync(cancellationToken);
            if (!boardExists) throw new NotFoundApplicationException($"Could not find board with ID: {boardId}");

            var boardUser = await _dbContext.BoardUsers.FirstOrDefaultAsync(bu => bu.BoardId == boardId && bu.UserId == currentUserId, cancellationToken);
            if (boardUser is null) return false;

            return _boardUserPermissionService.HasPermission(boardUser, permission);
        }

        public async Task<bool> HasPermissionForCard(int cardId, BoardUserPermission permission, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.UserId;

            var boardId = await _dbContext.Cards
                .Where(c => c.Id == cardId)
                .Select(c => c.BoardId)
                .FirstOrDefaultAsync(cancellationToken);
            if (boardId == default) throw new NotFoundApplicationException($"Could not find card with ID: {cardId}");

            var boardUser = await _dbContext.BoardUsers.FirstOrDefaultAsync(bu => bu.BoardId == boardId && bu.UserId == currentUserId, cancellationToken);
            if (boardUser is null) return false;

            return _boardUserPermissionService.HasPermission(boardUser, permission);
        }
    }
}
