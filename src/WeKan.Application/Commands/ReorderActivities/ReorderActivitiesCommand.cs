using MediatR;
using System.Collections.Generic;

namespace WeKan.Application.Commands.ReorderActivities
{
    public class ReorderActivitiesCommand : IRequest
    {
        public IEnumerable<int> ActivityIds { get; set; }
    }
}
