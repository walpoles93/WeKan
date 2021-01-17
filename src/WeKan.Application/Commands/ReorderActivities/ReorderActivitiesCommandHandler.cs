using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Activities;

namespace WeKan.Application.Commands.ReorderActivities
{
    public class ReorderActivitiesCommandHandler : IRequestHandler<ReorderActivitiesCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IActivityService _activityService;

        public ReorderActivitiesCommandHandler(IApplicationDbContext dbContext, IActivityService activityService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
        }

        public async Task<Unit> Handle(ReorderActivitiesCommand request, CancellationToken cancellationToken)
        {
            var activities = await _dbContext.Activities
                .Where(a => request.ActivityIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            if (!AllActivitesFound(request.ActivityIds, activities.Select(a => a.Id).ToList()))
                throw new NotFoundApplicationException($"Some or all activities could not be found. Ids: {string.Join(", ", request.ActivityIds)}");

            if (!AllActivitiesBelongToSameCard(activities))
                throw new InvalidOperationApplicationException($"Some activities dont belong to same card. Ids: {string.Join(", ", request.ActivityIds)}");

            // order activities sequence by the order of activityIds then calculate order properties
            var activityIds = request.ActivityIds.ToList();
            activities = activities.OrderBy(a => activityIds.IndexOf(a.Id)).ToList();
            _activityService.CalculateOrders(activities);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool AllActivitesFound(IEnumerable<int> expected, IEnumerable<int> actual)
        {
            var orderedExpected = expected.OrderBy(id => id);
            var orderedActual = actual.OrderBy(id => id);

            return orderedExpected.SequenceEqual(orderedActual);
        }

        private bool AllActivitiesBelongToSameCard(IEnumerable<Activity> activities)
        {
            var cardId = activities.First().CardId;

            return activities.All(a => a.CardId == cardId);
        }
    }
}
