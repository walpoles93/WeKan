using MediatR;

namespace WeKan.Application.Commands.JoinBoard
{
    public class JoinBoardCommand : IRequest
    {
        public string AccessCode { get; set; }
    }
}
