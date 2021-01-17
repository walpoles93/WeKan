using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Commands.EditCard
{
    public class EditCardCommandHandler : IRequestHandler<EditCardCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public EditCardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(EditCardCommand request, CancellationToken cancellationToken)
        {
            var card = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == request.CardId, cancellationToken);

            if (card is null) throw new NotFoundApplicationException($"Could not find card with Id: {request.CardId}");

            card.ChangeTitle(request.Title);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
