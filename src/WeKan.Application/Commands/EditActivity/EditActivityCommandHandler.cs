using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Commands.EditActivity
{
    public class EditActivityCommandHandler : IRequestHandler<EditActivityCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public EditActivityCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities.FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);

            if (activity is null) throw new NotFoundApplicationException($"Could not find activity with Id: {request.ActivityId}");

            activity.ChangeTitle(request.Title);
            activity.SetDescription(request.Description);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
