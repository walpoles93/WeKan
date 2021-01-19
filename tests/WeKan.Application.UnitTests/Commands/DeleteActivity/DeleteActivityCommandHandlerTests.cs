using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.DeleteActivity;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Activities;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.DeleteActivity
{
    public class DeleteActivityCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteActivityCommandHandler(null));
        }

        [Fact]
        public async Task Handle_ActivityIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(DeleteActivityCommandHandlerTests)}_{nameof(Handle_ActivityIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new DeleteActivityCommandHandler(context);
            var request = new DeleteActivityCommand(1);
            var cancellationToken = new CancellationToken();

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_ActivityIdExists_DeletesActivity()
        {
            var dbName = $"{nameof(DeleteActivityCommandHandlerTests)}_{nameof(Handle_ActivityIdExists_DeletesActivity)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var activity = Activity.Create("activity-title");
            context.Activities.Add(activity);
            await context.SaveChangesAsync(cancellationToken);

            var handler = new DeleteActivityCommandHandler(context);
            var request = new DeleteActivityCommand(1);
            await handler.Handle(request, cancellationToken);

            var activityFromDb = await context.Activities.FirstOrDefaultAsync(a => a.Id == 1);

            Assert.Null(activityFromDb);
        }
    }
}
