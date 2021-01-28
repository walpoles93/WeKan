using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.CreateBoard
{
    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, BoardCreatedDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUser _currentUser;
        private readonly IBoardUserFactory _boardUserFactory;

        public CreateBoardCommandHandler(IApplicationDbContext dbContext, ICurrentUser currentUser, IBoardUserFactory boardUserFactory)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _boardUserFactory = boardUserFactory ?? throw new ArgumentNullException(nameof(_boardUserFactory));
        }

        public async Task<BoardCreatedDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = Board.Create(request.Title);
            _dbContext.Boards.Add(board);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var boardUser = _boardUserFactory.CreateOwner(board.Id, _currentUser.UserId);
            _dbContext.BoardUsers.Add(boardUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BoardCreatedDto { BoardId = board.Id };
        }
    }
}
