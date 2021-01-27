using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.MoveActivityToCard;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.MoveActivityToCard
{
    public class MoveActivityToCardAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public MoveActivityToCardAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoveActivityToCardAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsCanEditCardAndCanEditActivity()
        {
            var cancellationToken = new CancellationToken();
            var expectedResult = true;
            var cardPermission = BoardUserPermission.CAN_EDIT_CARD;
            var activityPermission = BoardUserPermission.CAN_EDIT_ACTIVITY;

            _currentUserPermissionService
                .Setup(s => s.HasPermissionForCard(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == cardPermission),
                    cancellationToken))
                .ReturnsAsync(expectedResult);
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForActivity(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == activityPermission),
                    cancellationToken))
                .ReturnsAsync(expectedResult);

            var authorizer = new MoveActivityToCardAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new MoveActivityToCardCommand(), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForCard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == cardPermission), cancellationToken), Times.Once);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForActivity(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == activityPermission), cancellationToken), Times.Once);
        }
    }
}
