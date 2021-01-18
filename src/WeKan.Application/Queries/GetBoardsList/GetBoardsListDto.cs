using System.Collections.Generic;

namespace WeKan.Application.Queries.GetBoardsList
{
    public class GetBoardsListDto
    {
        public IEnumerable<Board> Boards { get; set; }

        public class Board
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
