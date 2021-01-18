using MediatR;

namespace WeKan.Application.Commands.DeleteCard
{
    public class DeleteCardCommand : IRequest
    {
        public DeleteCardCommand(int cardId)
        {
            CardId = cardId;
        }

        public int CardId { get; set; }
    }
}
