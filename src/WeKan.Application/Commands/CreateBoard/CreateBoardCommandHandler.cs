using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;

namespace WeKan.Application.Commands.CreateBoard
{
    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateBoardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = Board.Create(request.Title);

            _dbContext.Boards.Add(board);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
