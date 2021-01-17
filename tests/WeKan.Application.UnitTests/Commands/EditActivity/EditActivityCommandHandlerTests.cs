using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.EditActivity;
using WeKan.Application.Common.Exceptions;
using WeKan.Domain.Activities;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.EditActivity
{
    public class EditActivityCommandHandlerTests
    {
        [Fact]
        public void Ctor_IApplicationDbContextNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditActivityCommandHandler(null));
        }

        [Fact]
        public async Task Handle_ActivityIdNotExists_ThrowsNotFoundApplicationException()
        {
            var dbName = $"{nameof(EditActivityCommandHandlerTests)}_{nameof(Handle_ActivityIdNotExists_ThrowsNotFoundApplicationException)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var handler = new EditActivityCommandHandler(context);
            var request = new EditActivityCommand { ActivityId = 1, Title = "test-title", Description = "test-description" };
            var cancellationToken = new CancellationToken();

            Task<Unit> action() => handler.Handle(request, cancellationToken);

            await Assert.ThrowsAsync<NotFoundApplicationException>(action);
        }

        [Fact]
        public async Task Handle_ActivityIdExists_EditsActivity()
        {
            var dbName = $"{nameof(EditActivityCommandHandlerTests)}_{nameof(Handle_ActivityIdExists_EditsActivity)}";
            using var context = TestApplicationDbContext.Create(dbName);
            var cancellationToken = new CancellationToken();

            var activity = Activity.Create("test-title");
            context.Activities.Add(activity);
            await context.SaveChangesAsync(cancellationToken);

            var editedTitle = "edited-title";
            var editedDescription = "edited-description";
            var handler = new EditActivityCommandHandler(context);
            var request = new EditActivityCommand { ActivityId = 1, Title = editedTitle, Description = editedDescription };

            await handler.Handle(request, cancellationToken);

            var activityFromDb = await context.Activities.FirstOrDefaultAsync(a => a.Id == 1, cancellationToken);

            Assert.NotNull(activityFromDb);
            Assert.Equal(editedTitle, activityFromDb.Title);
            Assert.Equal(editedDescription, activityFromDb.Description);
        }
    }
}
