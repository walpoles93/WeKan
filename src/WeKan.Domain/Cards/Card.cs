using System;
using System.Collections.Generic;
using WeKan.Domain.Activities;
using WeKan.Domain.Common.Models;

namespace WeKan.Domain.Cards
{
    public class Card : Entity
    {
        public static Card Create(string title, int? order = null) => new Card(title, order);

        private Card(string title, int? order)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException(nameof(title));

            Title = title;
            Order = order;
        }

        private Card() { }

        public int BoardId { get; private set; } // populated by EF Core

        public string Title { get; private set; } = string.Empty;

        public int? Order { get; private set; }

        private readonly List<Activity> _activities = new List<Activity>();
        public IReadOnlyCollection<Activity> Activities => _activities;

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException(nameof(title));

            Title = title;
        }

        public void AddActivity(Activity activity)
        {
            if (activity is null) throw new ArgumentNullException(nameof(activity));

            _activities.Add(activity);
        }

        public bool RemoveActivity(Activity activity)
        {
            return _activities.Remove(activity);
        }

        internal void ChangeOrder(int order)
        {
            Order = order;
        }
    }
}
