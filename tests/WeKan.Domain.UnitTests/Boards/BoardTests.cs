using System;
using System.Linq;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Domain.UnitTests.Boards
{
    public class BoardTests
    {
        [Fact]
        public void Ctor_TitleLengthGreaterThanZero_ReturnsBoard()
        {
            var title = "test-title";
            var accessCode = "accessCode";
            var board = new Board(title, accessCode);

            Assert.IsType<Board>(board);
            Assert.NotNull(board);
            Assert.Equal(title, board.Title);
            Assert.Equal(accessCode, board.AccessCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_TitleNullOrEmpty_ThrowsArgumentException(string title)
        {
            Board action() => new Board(title, "accessCode");

            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_AccessCodeNullOrEmpty_ThrowsArgumentException(string accessCode)
        {
            Board action() => new Board("title", accessCode);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Ctor_CardsCollectionIsEmpty()
        {
            var title = "test-title";
            var board = new Board(title, "accessCode");

            Assert.NotNull(board.Cards);
            Assert.Empty(board.Cards);
        }

        [Fact]
        public void ChangeTitle_TitleLengthGreaterThanZero_SetsTitle()
        {
            var oldTitle = "old-title";
            var newTitle = "new-title";
            var board = new Board(oldTitle, "accessCode");

            board.ChangeTitle(newTitle);

            Assert.Equal(newTitle, board.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ChangeTitle_TitleNullOrEmpty_ThrowsArgumentException(string newTitle)
        {
            var oldTitle = "old-title";
            var board = new Board(oldTitle, "accessCode");

            void action() => board.ChangeTitle(newTitle);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void AddCard_CardInstance_AddsCardToCardsCollection()
        {
            var card = Card.Create("card-title");
            var title = "test-title";
            var board = new Board(title, "accessCode");

            board.AddCard(card);

            Assert.Single(board.Cards);
            Assert.Equal(card, board.Cards.First());
        }

        [Fact]
        public void AddCard_CardNull_ThrowsArgumentNullException()
        {
            Card card = null;
            var title = "test-title";
            var board = new Board(title, "accessCode");

            void action() => board.AddCard(card);

            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
