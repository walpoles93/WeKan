using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.EditCard;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.EditCard
{
    public class EditCardAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public EditCardAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditCardAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsHasPermissionForCard_CanEditCard()
        {
            var permission = BoardUserPermission.CAN_EDIT_CARD;
            var expectedResult = true;
            var cancellationToken = new CancellationToken();
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForCard(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == permission),
                    cancellationToken))
                .ReturnsAsync(expectedResult);

            var authorizer = new EditCardAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new EditCardCommand(), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForCard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
