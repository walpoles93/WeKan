using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Commands.EditBoard
{
    public class EditBoardCommandHandler : IRequestHandler<EditBoardCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public EditBoardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(EditBoardCommand request, CancellationToken cancellationToken)
        {
            var board = await _dbContext.Boards.FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken);

            if (board is null) throw new NotFoundApplicationException($"Could not find board with Id: {request.BoardId}");

            board.ChangeTitle(request.Title);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
