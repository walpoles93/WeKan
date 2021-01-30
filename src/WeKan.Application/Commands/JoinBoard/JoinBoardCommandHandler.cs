using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.JoinBoard
{
    public class JoinBoardCommandHandler : IRequestHandler<JoinBoardCommand, BoardJoinedDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardUserFactory _boardUserFactory;

        public JoinBoardCommandHandler(IApplicationDbContext dbContext, ICurrentUser currentUser, IBoardUserFactory boardUserFactory)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _boardUserFactory = boardUserFactory ?? throw new ArgumentNullException(nameof(boardUserFactory));
        }

        public async Task<BoardJoinedDto> Handle(JoinBoardCommand request, CancellationToken cancellationToken)
        {
            var boardId = await _dbContext.Boards
                .Where(b => b.AccessCode == request.AccessCode)
                .Select(b => b.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (boardId == default) throw new NotFoundApplicationException($"Board could not be found with access code: {request.AccessCode}");

            var boardUser = _boardUserFactory.CreateCollaborator(boardId, _currentUser.UserId);
            _dbContext.BoardUsers.Add(boardUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BoardJoinedDto { BoardId = boardId };

        }
    }
}
