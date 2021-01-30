using System;

namespace WeKan.Domain.Boards
{
    public interface IBoardFactory
    {
        Board Create(string title);
    }

    public class BoardFactory : IBoardFactory
    {
        public Board Create(string title)
        {
            var accessCode = GenerateAccessCode();

            return new Board(title, accessCode);
        }

        private string GenerateAccessCode()
        {
            var ticks = new DateTime(2020, 1, 1).Ticks;

            return (DateTime.Now.Ticks - ticks).ToString("x");
        }
    }
}
