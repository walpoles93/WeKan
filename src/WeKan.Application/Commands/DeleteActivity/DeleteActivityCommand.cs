using MediatR;

namespace WeKan.Application.Commands.DeleteActivity
{
    public class DeleteActivityCommand : IRequest
    {
        public DeleteActivityCommand(int activityId)
        {
            ActivityId = activityId;
        }

        public int ActivityId { get; set; }
    }
}
