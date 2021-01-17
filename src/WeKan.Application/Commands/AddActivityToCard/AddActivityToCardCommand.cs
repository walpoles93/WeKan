using MediatR;

namespace WeKan.Application.Commands.AddActivityToCard
{
    public class AddActivityToCardCommand : IRequest
    {
        public string Title { get; set; }
        public int CardId { get; set; }
    }
}
