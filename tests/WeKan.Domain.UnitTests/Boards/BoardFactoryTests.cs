using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Domain.UnitTests.Boards
{
    public class BoardFactoryTests
    {
        [Fact]
        public void Create_ReturnsBoardWithTitle()
        {
            var title = "test-title";
            var factory = new BoardFactory();
            var board = factory.Create(title);

            Assert.Equal(title, board.Title);
        }

        [Fact]
        public void Create_ReturnsBoardWithAccessCode()
        {
            var title = "test-title";
            var factory = new BoardFactory();
            var board = factory.Create(title);

            Assert.NotNull(board.AccessCode);
            Assert.NotEmpty(board.AccessCode);
        }
    }
}
