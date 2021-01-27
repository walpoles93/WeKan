using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.AddCardToBoard;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.AddCardToBoard
{
    public class AddCardToBoardAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public AddCardToBoardAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AddCardToBoardAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsHasPermissionForBoard_CanCreateCard()
        {
            var permission = BoardUserPermission.CAN_CREATE_CARD;
            var expectedResult = true;
            var cancellationToken = new CancellationToken();
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForBoard(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == permission),
                    new CancellationToken()))
                .ReturnsAsync(expectedResult);

            var authorizer = new AddCardToBoardAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new AddCardToBoardCommand(), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForBoard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
