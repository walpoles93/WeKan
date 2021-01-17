using System;
using WeKan.Domain.Cards;
using WeKan.Domain.Common.Models;

namespace WeKan.Domain.Activities
{
    public class Activity : Entity
    {
        public static Activity Create(string title, int? order = null) => new Activity(title, order);

        private Activity(string title, int? order)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException(nameof(title));

            Title = title;
            Order = order;
        }

        private Activity() { }

        public int CardId { get; private set; } // populated by EF Core

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public int? Order { get; private set; }

        public void TransferTo(Card card)
        {
            if (card is null) throw new ArgumentNullException(nameof(card));

            CardId = card.Id;
            card.AddActivity(this);
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException(nameof(title));

            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description ?? string.Empty;
        }

        public void ClearOrder()
        {
            Order = null;
        }

        internal void ChangeOrder(int order)
        {
            Order = order;
        }
    }
}
