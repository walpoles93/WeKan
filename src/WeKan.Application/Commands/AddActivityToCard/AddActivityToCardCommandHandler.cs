using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Activities;

namespace WeKan.Application.Commands.AddActivityToCard
{
    public class AddActivityToCardCommandHandler : IRequestHandler<AddActivityToCardCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public AddActivityToCardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(AddActivityToCardCommand request, CancellationToken cancellationToken)
        {
            var activity = Activity.Create(request.Title);
            var card = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == request.CardId, cancellationToken);

            if (card is null) throw new NotFoundApplicationException($"Could not find card with Id: ${request.CardId}");

            card.AddActivity(activity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
