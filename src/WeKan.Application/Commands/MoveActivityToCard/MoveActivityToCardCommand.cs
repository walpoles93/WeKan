using MediatR;

namespace WeKan.Application.Commands.MoveActivityToCard
{
    public class MoveActivityToCardCommand : IRequest
    {
        public int ActivityId { get; set; }
        public int CardId { get; set; }
    }
}
