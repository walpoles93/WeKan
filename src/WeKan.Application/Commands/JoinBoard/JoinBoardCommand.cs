using MediatR;

namespace WeKan.Application.Commands.JoinBoard
{
    public class JoinBoardCommand : IRequest<BoardJoinedDto>
    {
        public string AccessCode { get; set; }
    }
}
