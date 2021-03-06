﻿using System;
using System.Collections.Generic;
using WeKan.Domain.Cards;
using WeKan.Domain.Common.Models;

namespace WeKan.Domain.Boards
{
    public class Board : Entity
    {
        internal Board(string title, string accessCode)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException(nameof(title));
            if (string.IsNullOrEmpty(accessCode)) throw new ArgumentException(nameof(accessCode));

            Title = title;
            AccessCode = accessCode;
        }

        private Board() { }

        public string Title { get; private set; } = string.Empty;
        public string AccessCode { get; private set; } = string.Empty;

        private readonly List<Card> _cards = new List<Card>();
        public IReadOnlyCollection<Card> Cards => _cards;

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException(nameof(title));

            Title = title;
        }

        public void AddCard(Card card)
        {
            if (card is null) throw new ArgumentNullException(nameof(card));

            _cards.Add(card);
        }
    }
}
