﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Queries.GetBoard
{
    public class GetBoardQueryHandler : IRequestHandler<GetBoardQuery, GetBoardDto>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetBoardQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<GetBoardDto> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            var board = await GetQuery(request.BoardId).FirstOrDefaultAsync(cancellationToken);

            if (board is null) throw new NotFoundApplicationException($"Could not find board with Id: {request.BoardId}");

            var boardUserType = await _dbContext.BoardUsers
                .Where(bu => bu.BoardId == request.BoardId)
                .Select(bu => bu.Type.ToString().ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            board.BoardUserType = boardUserType;

            return board;
        }

        private IQueryable<GetBoardDto> GetQuery(int boardId)
        {
            return _dbContext.Boards
                .Where(b => b.Id == boardId)
                .Select(b => new GetBoardDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AccessCode = b.AccessCode,
                    CreatedByUserId = b.CreatedByUserId,
                    Cards = b.Cards.OrderBy(c => c.Order).Select(c => new GetBoardDto.Card
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Activities = c.Activities.OrderBy(a => a.Order).Select(a => new GetBoardDto.Activity
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                        }),
                    }),
                });
        }
    }
}
