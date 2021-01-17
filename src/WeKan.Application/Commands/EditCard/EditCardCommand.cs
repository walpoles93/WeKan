using MediatR;

namespace WeKan.Application.Commands.EditCard
{
    public class EditCardCommand : IRequest
    {
        public int CardId { get; set; }
        public string Title { get; set; }
    }
}
