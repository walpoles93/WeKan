using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Cards;

namespace WeKan.Application.Commands.ReorderCards
{
    public class ReorderCardsCommandHandler : IRequestHandler<ReorderCardsCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICardService _cardService;

        public ReorderCardsCommandHandler(IApplicationDbContext dbContext, ICardService cardService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
        }

        public async Task<Unit> Handle(ReorderCardsCommand request, CancellationToken cancellationToken)
        {
            var cards = await _dbContext.Cards
                .Where(c => request.CardIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (!AllCardsFound(request.CardIds, cards.Select(c => c.Id).ToList()))
                throw new NotFoundApplicationException($"Some or all cards could not be found. Ids: {string.Join(", ", request.CardIds)}");

            if (!AllCardsBelongToSameBoard(cards))
                throw new InvalidOperationApplicationException($"Some card dont belong to same board. Ids: {string.Join(", ", request.CardIds)}");

            // order cards sequence by the order of cardIds then calculate order properties
            var cardIds = request.CardIds.ToList();
            cards = cards.OrderBy(c => cardIds.IndexOf(c.Id)).ToList();
            _cardService.CalculateOrders(cards);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool AllCardsFound(IEnumerable<int> expected, IEnumerable<int> actual)
        {
            var orderedExpected = expected.OrderBy(id => id);
            var orderedActual = actual.OrderBy(id => id);

            return orderedExpected.SequenceEqual(orderedActual);
        }

        private bool AllCardsBelongToSameBoard(IEnumerable<Card> cards)
        {
            var boardId = cards.First().BoardId;

            return cards.All(c => c.BoardId == boardId);
        }
    }
}
