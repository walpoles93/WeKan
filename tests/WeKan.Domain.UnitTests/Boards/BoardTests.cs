using System;
using System.Linq;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Domain.Tests.Boards
{
    public class BoardTests
    {
        [Fact]
        public void Create_TitleLengthGreaterThanZero_ReturnsBoard()
        {
            var title = "test-title";
            var board = Board.Create(title);

            Assert.IsType<Board>(board);
            Assert.NotNull(board);
            Assert.Equal(title, board.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_TitleNullOrEmpty_ThrowsArgumentException(string title)
        {
            Board action() => Board.Create(title);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_CardsCollectionIsEmpty()
        {
            var title = "test-title";
            var board = Board.Create(title);

            Assert.NotNull(board.Cards);
            Assert.Empty(board.Cards);
        }

        [Fact]
        public void ChangeTitle_TitleLengthGreaterThanZero_SetsTitle()
        {
            var oldTitle = "old-title";
            var newTitle = "new-title";
            var board = Board.Create(oldTitle);

            board.ChangeTitle(newTitle);

            Assert.Equal(newTitle, board.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ChangeTitle_TitleNullOrEmpty_ThrowsArgumentException(string newTitle)
        {
            var oldTitle = "old-title";
            var board = Board.Create(oldTitle);

            void action() => board.ChangeTitle(newTitle);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void AddCard_CardInstance_AddsCardToCardsCollection()
        {
            var card = Card.Create("card-title");
            var title = "test-title";
            var board = Board.Create(title);

            board.AddCard(card);

            Assert.Single(board.Cards);
            Assert.Equal(card, board.Cards.First());
        }

        [Fact]
        public void AddCard_CardNull_ThrowsArgumentNullException()
        {
            Card card = null;
            var title = "test-title";
            var board = Board.Create(title);

            void action() => board.AddCard(card);

            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
