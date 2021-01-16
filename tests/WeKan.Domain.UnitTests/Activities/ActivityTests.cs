using System;
using WeKan.Domain.Activities;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Domain.UnitTests.Activities
{
    public class ActivityTests
    {
        [Fact]
        public void Create_TitleLengthGreaterThanZero_ReturnsActivity()
        {
            var title = "test-title";
            var activity = Activity.Create(title);

            Assert.IsType<Activity>(activity);
            Assert.NotNull(activity);
            Assert.Equal(title, activity.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_TitleNullOrEmpty_ThrowsArgumentException(string title)
        {
            Activity action() => Activity.Create(title);

            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void Create_SetOrder_SetsOrder(int? order)
        {
            var title = "test-title";
            var activity = Activity.Create(title, order);

            Assert.Equal(order, activity.Order);
        }

        [Fact]
        public void TransferTo_CardInstance_SetsCardIdAndCallsCardAddActivity()
        {
            var title = "test-title";
            var activity = Activity.Create(title);
            var cardId = 1;
            var card = Card.Create("card-title");
            card.Id = cardId;

            activity.TransferTo(card);

            Assert.Equal(cardId, activity.CardId);
        }

        [Fact]
        public void TransferTo_CardNull_ThrowsArgumentNullException()
        {
            var title = "test-title";
            var activity = Activity.Create(title);

            void action() => activity.TransferTo(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ChangeTitle_TitleLengthGreaterThanZero_SetsTitle()
        {
            var oldTitle = "old-title";
            var newTitle = "new-title";
            var activity = Activity.Create(oldTitle);

            activity.ChangeTitle(newTitle);

            Assert.Equal(newTitle, activity.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ChangeTitle_TitleNullOrEmpty_ThrowsArgumentException(string newTitle)
        {
            var title = "test-title";
            var activity = Activity.Create(title);

            void action() => activity.ChangeTitle(newTitle);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void SetDescription_DescriptionInstance_SetsDescription()
        {
            var title = "test-title";
            var description = "test-description";
            var activity = Activity.Create(title);

            activity.SetDescription(description);

            Assert.Equal(description, activity.Description);
        }

        [Fact]
        public void SetDescription_DescriptionNull_SetsDescriptionEmptyString()
        {
            var title = "test-title";
            var description = "test-description";
            var activity = Activity.Create(title);
            activity.SetDescription(description);

            activity.SetDescription(null);

            Assert.Equal(string.Empty, activity.Description);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(0)]
        public void ChangeOrder_SetsOrder(int order)
        {
            var title = "test-title";
            var activity = Activity.Create(title);

            activity.ChangeOrder(order);

            Assert.Equal(order, activity.Order);
        }
    }
}
