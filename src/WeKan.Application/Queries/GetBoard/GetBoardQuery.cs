using MediatR;

namespace WeKan.Application.Queries.GetBoard
{
    public class GetBoardQuery : IRequest<GetBoardDto>
    {
        public GetBoardQuery(int boardId)
        {
            BoardId = boardId;
        }

        public int BoardId { get; set; }
    }
}
