using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Queries.GetBoardsList
{
    public class GetBoardsListQueryHandler : IRequestHandler<GetBoardsListQuery, GetBoardsListDto>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetBoardsListQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<GetBoardsListDto> Handle(GetBoardsListQuery request, CancellationToken cancellationToken)
        {
            var boards = await GetQuery().ToListAsync(cancellationToken);

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
