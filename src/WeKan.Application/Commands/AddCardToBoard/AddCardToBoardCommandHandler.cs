using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Cards;

namespace WeKan.Application.Commands.AddCardToBoard
{
    public class AddCardToBoardCommandHandler : IRequestHandler<AddCardToBoardCommand, CardCreatedDto>
    {
        private readonly IApplicationDbContext _dbContext;

        public AddCardToBoardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CardCreatedDto> Handle(AddCardToBoardCommand request, CancellationToken cancellationToken)
        {
            var card = Card.Create(request.Title);
            var board = await _dbContext.Boards.FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken);

            if (board is null) throw new NotFoundApplicationException($"Could not find board with Id: {request.BoardId}");

            board.AddCard(card);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CardCreatedDto { CardId = card.Id, BoardId = card.BoardId };
        }
    }
}
