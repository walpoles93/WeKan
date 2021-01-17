using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Commands.DeleteBoard
{
    public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteBoardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            var board = await _dbContext.Boards.FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken);

            if (board is null) throw new NotFoundApplicationException($"Could not find board with Id: {request.BoardId}");

            _dbContext.Boards.Remove(board);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
