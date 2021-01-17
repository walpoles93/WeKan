using MediatR;

namespace WeKan.Application.Commands.DeleteBoard
{
    public class DeleteBoardCommand : IRequest
    {
        public int BoardId { get; set; }
    }
}
