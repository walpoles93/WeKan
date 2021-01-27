using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Queries.GetBoardsList
{
    public class GetBoardsListQueryHandler : IRequestHandler<GetBoardsListQuery, GetBoardsListDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetBoardsListQueryHandler(IApplicationDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<GetBoardsListDto> Handle(GetBoardsListQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.UserId;
            var allowedBoardIds = await _dbContext.BoardUsers
                .Where(bu => bu.UserId == _currentUser.UserId)
                .Where(bu => bu.Type == BoardUserType.Owner || bu.Type == BoardUserType.Collaborator)
                .Select(bu => bu.BoardId)
                .ToListAsync(cancellationToken);
            var boards = await GetQuery()
                .Where(b => allowedBoardIds.Contains(b.Id))
                .ToListAsync(cancellationToken);

            return new GetBoardsListDto { Boards = boards };
        }

        private IQueryable<GetBoardsListDto.Board> GetQuery()
        {
            return _dbContext.Boards
                .Select(b => new GetBoardsListDto.Board
                {
                    Id = b.Id,
                    Title = b.Title,
                });
        }
    }
}
