using System;
using System.Collections.Generic;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Domain.Tests.Cards
{
    public class CardServiceTests
    {
        [Fact]
        public void CalculateOrders_EnumerableInstance_CallsChangeOrderFromSequence()
        {
            var service = new CardService();
            var card1 = Card.Create("card1-title");
            var card2 = Card.Create("card2-title");
            var cards = new List<Card> { card1, card2 };

            service.CalculateOrders(cards);

            Assert.Equal(0, card1.Order);
            Assert.Equal(1, card2.Order);
        }

        [Fact]
        public void CalculateOrders_EnumerableNull_ThrowsNullArgumentException()
        {
            var service = new CardService();

            void action() => service.CalculateOrders(null);

            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
