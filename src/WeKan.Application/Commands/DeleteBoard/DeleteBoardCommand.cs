using MediatR;

namespace WeKan.Application.Commands.DeleteBoard
{
    public class DeleteBoardCommand : IRequest
    {
        public DeleteBoardCommand(int boardId)
        {
            BoardId = boardId;
        }

        public int BoardId { get; set; }
    }
}
