using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Commands.MoveActivityToCard
{
    public class MoveActivityToCardCommandHandler : IRequestHandler<MoveActivityToCardCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public MoveActivityToCardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(MoveActivityToCardCommand request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);
            var card = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == request.CardId, cancellationToken);

            if (activity is null) throw new NotFoundApplicationException($"Could not find activity with Id: {request.ActivityId}");
            if (card is null) throw new NotFoundApplicationException($"Could not find activity with Id: {request.CardId}");

            activity.TransferTo(card);
            activity.ClearOrder();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
