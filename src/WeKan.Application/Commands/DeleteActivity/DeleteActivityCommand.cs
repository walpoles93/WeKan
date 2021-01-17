using MediatR;

namespace WeKan.Application.Commands.DeleteActivity
{
    public class DeleteActivityCommand : IRequest
    {
        public int ActivityId { get; set; }
    }
}
