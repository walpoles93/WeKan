using MediatR;

namespace WeKan.Application.Commands.DeleteCard
{
    public class DeleteCardCommand : IRequest
    {
        public int CardId { get; set; }
    }
}
