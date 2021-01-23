using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeKan.Application.Commands.AddCardToBoard;
using WeKan.Application.Commands.DeleteCard;
using WeKan.Application.Commands.EditCard;
using WeKan.Application.Commands.ReorderCards;

namespace WeKan.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<Unit> AddCardToBoard(AddCardToBoardCommand command) => await _mediator.Send(command);

        [HttpPut]
        public async Task<Unit> EditCard(EditCardCommand command) => await _mediator.Send(command);

        [HttpPut("Reorder")]
        public async Task<Unit> RedorderCards(ReorderCardsCommand command) => await _mediator.Send(command);

        [HttpDelete("{cardId:int}")]
        public async Task<Unit> DeleteCard(int cardId) => await _mediator.Send(new DeleteCardCommand(cardId));
    }
}
