using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.AddActivityToCard;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.AddActivityToCard
{
    public class AddActivityToCardAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public AddActivityToCardAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AddActivityToCardAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsHasPermissionForCard_CanCreateActivity()
        {
            var permission = BoardUserPermission.CAN_CREATE_ACTIVITY;
            var expectedResult = true;
            var cancellationToken = new CancellationToken();
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForCard(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == permission),
                    new CancellationToken()))
                .ReturnsAsync(expectedResult);

            var authorizer = new AddActivityToCardAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new AddActivityToCardCommand(), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForCard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
