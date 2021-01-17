using MediatR;

namespace WeKan.Application.Commands.EditActivity
{
    public class EditActivityCommand : IRequest
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
