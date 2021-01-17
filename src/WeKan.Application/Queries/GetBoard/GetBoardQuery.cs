using MediatR;

namespace WeKan.Application.Queries.GetBoard
{
    public class GetBoardQuery : IRequest<GetBoardDto>
    {
        public int BoardId { get; set; }
    }
}
