using System.Collections.Generic;

namespace WeKan.Application.Queries.GetBoard
{
    public class GetBoardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Card> Cards { get; set; }

        public class Card
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public IEnumerable<Activity> Activities { get; set; }
        }

        public class Activity
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
}
