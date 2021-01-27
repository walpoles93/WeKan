using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.ReorderActivities;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.ReorderActivities
{
    public class ReorderActivitiesAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public ReorderActivitiesAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReorderActivitiesAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_ActivityIdsNull_ReturnsTrue()
        {
            var authorizer = new ReorderActivitiesAuthorizer(_currentUserPermissionService.Object);
            var request = new ReorderActivitiesCommand { ActivityIds = null };
            var cancellationToken = new CancellationToken();

            var result = await authorizer.Authorise(request, cancellationToken);

            Assert.True(result);
        }

        [Fact]
        public async Task Authorise_ActivityIdsEmpty_ReturnsTrue()
        {
            var authorizer = new ReorderActivitiesAuthorizer(_currentUserPermissionService.Object);
            var request = new ReorderActivitiesCommand { ActivityIds = new List<int>() };
            var cancellationToken = new CancellationToken();

            var result = await authorizer.Authorise(request, cancellationToken);

            Assert.True(result);
        }

        [Fact]
        public async Task Authorise_FirstActivityId_CallsHasPermissionForActivity_CanEditActivity()
        {
            var permission = BoardUserPermission.CAN_EDIT_ACTIVITY;
            var expectedResult = true;
            var cancellationToken = new CancellationToken();
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForActivity(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == permission),
                    cancellationToken))
                .ReturnsAsync(expectedResult);

            var authorizer = new ReorderActivitiesAuthorizer(_currentUserPermissionService.Object);
            var request = new ReorderActivitiesCommand { ActivityIds = new List<int> { 1 } };
            var result = await authorizer.Authorise(request, cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForActivity(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
