using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Queries.GetBoard;
using WeKan.Domain.Activities;
using WeKan.Domain.Boards;
using WeKan.Domain.Cards;
using Xunit;

namespace WeKan.Application.UnitTests.Queries.GetBoard
{
    public class GetBoardQueryHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetBoardQueryHandler(null));
        }

        [Fact]
        public async Task Handle_BoardIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(GetBoardQueryHandlerTests)}_{nameof(Handle_BoardIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();
            var handler = new GetBoardQueryHandler(context);
            var request = new GetBoardQuery { BoardId = 1 };

            Task<GetBoardDto> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_BoardIdExists_ReturnDto()
        {
            var dbName = $"{nameof(GetBoardQueryHandlerTests)}_{nameof(Handle_BoardIdExists_ReturnDto)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var board = Board.Create("board-title");
            var card = Card.Create("card-title");
            var activity = Activity.Create("activity-title");
            board.AddCard(card);
            card.AddActivity(activity);
            context.Boards.Add(board);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new GetBoardQueryHandler(context);
            var request = new GetBoardQuery { BoardId = 1 };

            var dto = await handler.Handle(request, cancellationToken);
            var dtoCard = dto.Cards.FirstOrDefault();
            var dtoActivity = dtoCard.Activities.FirstOrDefault();

            Assert.NotNull(dto);
            Assert.Equal(1, dto.Id);
            Assert.NotNull(dto.Cards);
            Assert.Single(dto.Cards);
            Assert.Equal(1, dtoCard.Id);
            Assert.NotNull(dtoCard.Activities);
            Assert.Single(dtoCard.Activities);
            Assert.Equal(1, dtoActivity.Id);
        }
    }
}
