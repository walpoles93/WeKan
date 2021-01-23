using MediatR;

namespace WeKan.Application.Commands.CreateBoard
{
    public class CreateBoardCommand : IRequest<BoardCreatedDto>
    {
        public string Title { get; set; }
    }
}
