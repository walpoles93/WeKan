using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeKan.Application.Commands.CreateBoard;
using WeKan.Application.Commands.DeleteBoard;
using WeKan.Application.Commands.EditBoard;
using WeKan.Application.Queries.GetBoard;
using WeKan.Application.Queries.GetBoardsList;

namespace WeKan.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardsController
    {
        private readonly IMediator _mediator;

        public BoardsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<Unit> CreateBoard(CreateBoardCommand command) => await _mediator.Send(command);

        [HttpGet]
        public async Task<GetBoardsListDto> GetBoards() => await _mediator.Send(new GetBoardsListQuery());

        [HttpGet("{boardId:int}")]
        public async Task<GetBoardDto> GetBoard(int boardId) => await _mediator.Send(new GetBoardQuery(boardId));

        [HttpPut]
        public async Task<Unit> EditBoard(EditBoardCommand command) => await _mediator.Send(command);

        [HttpDelete("{boardId:int}")]
        public async Task<Unit> DeleteBoard(int boardId) => await _mediator.Send(new DeleteBoardCommand(boardId));
    }
}
