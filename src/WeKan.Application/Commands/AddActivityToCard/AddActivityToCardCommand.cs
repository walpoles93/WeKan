using MediatR;

namespace WeKan.Application.Commands.AddActivityToCard
{
    public class AddActivityToCardCommand : IRequest<ActivityCreatedDto>
    {
        public string Title { get; set; }
        public int CardId { get; set; }
    }
}
