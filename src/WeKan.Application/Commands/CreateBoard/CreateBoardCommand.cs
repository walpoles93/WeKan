using MediatR;

namespace WeKan.Application.Commands.CreateBoard
{
    public class CreateBoardCommand : IRequest
    {
        public string Title { get; set; }
    }
}
