using System;
using System.Collections.Generic;

namespace WeKan.Domain.Cards
{
    public interface ICardService
    {
        void CalculateOrders(IEnumerable<Card> cards);
    }

    public class CardService : ICardService
    {
        /// <summary>
        /// Takes a collection of cards and changes the order property
        /// of each card to match its position in the collection
        /// </summary>
        /// <param name="cards"></param>
        public void CalculateOrders(IEnumerable<Card> cards)
        {
            if (cards is null) throw new ArgumentNullException(nameof(cards));

            var order = 0;
            foreach (var card in cards)
            {
                card.ChangeOrder(order);
                order += 1;
            }
        }
    }
}
