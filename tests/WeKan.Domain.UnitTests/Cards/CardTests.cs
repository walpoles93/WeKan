using System;
using System.Linq;
using WeKan.Domain.Activities;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Domain.UnitTests.Cards
{
    public class CardTests
    {
        [Fact]
        public void Create_TitleLengthGreaterThanZero_ReturnsCard()
        {
            var title = "test-title";
            var card = Card.Create(title);

            Assert.IsType<Card>(card);
            Assert.NotNull(card);
            Assert.Equal(title, card.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0)]
        public void Create_SetOrder_SetsOrder(int? order)
        {
            var title = "test-title";
            var card = Card.Create(title, order);

            Assert.Equal(order, card.Order);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_TitleNullOrEmpty_ThrowsArgumentException(string title)
        {
            Card action() => Card.Create(title);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Create_ActivitiesCollectionIsEmpty()
        {
            var title = "test-title";
            var card = Card.Create(title);

            Assert.NotNull(card.Activities);
            Assert.Empty(card.Activities);
        }

        [Fact]
        public void ChangeTitle_TitleLengthGreaterThanZero_SetsTitle()
        {
            var oldTitle = "old-title";
            var card = Card.Create(oldTitle);
            var newTitle = "new-title";

            card.ChangeTitle(newTitle);

            Assert.Equal(newTitle, card.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ChangeTitle_TitleNullOrEmpty_ThrowsArgumentException(string newTitle)
        {
            var oldTitle = "old-title";
            var card = Card.Create(oldTitle);

            void action() => card.ChangeTitle(newTitle);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void AddActivity_ActivityInstance_AddsActivityToCollection()
        {
            var activity = Activity.Create("activity-title");
            var title = "test-title";
            var card = Card.Create(title);

            card.AddActivity(activity);

            Assert.Single(card.Activities);
            Assert.Equal(activity, card.Activities.First());
        }

        [Fact]
        public void AddActivity_ActivityNull_ThrowsArgumentNullException()
        {
            Activity activity = null;
            var title = "test-title";
            var card = Card.Create(title);

            void action() => card.AddActivity(activity);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0)]
        public void ChangeOrder_SetsOrder(int order)
        {
            var title = "test-title";
            var card = Card.Create(title);

            card.ChangeOrder(order);

            Assert.Equal(order, card.Order);
        }
    }
}
