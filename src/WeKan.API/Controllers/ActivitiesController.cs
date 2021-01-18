using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeKan.Application.Commands.AddActivityToCard;
using WeKan.Application.Commands.DeleteActivity;
using WeKan.Application.Commands.EditActivity;
using WeKan.Application.Commands.MoveActivityToCard;
using WeKan.Application.Commands.ReorderActivities;

namespace WeKan.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<Unit> AddActivityToCard(AddActivityToCardCommand command) => await _mediator.Send(command);

        [HttpPut]
        public async Task<Unit> EditActivity(EditActivityCommand command) => await _mediator.Send(command);

        [HttpPut("MoveToCard")]
        public async Task<Unit> MoveActivityToCard(MoveActivityToCardCommand command) => await _mediator.Send(command);

        [HttpPut("Reorder")]
        public async Task<Unit> ReorderActivities(ReorderActivitiesCommand command) => await _mediator.Send(command);

        [HttpDelete("{activityId:int}")]
        public async Task<Unit> DeleteActivity(int activityId) => await _mediator.Send(new DeleteActivityCommand(activityId));
    }
}
