using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.EditBoard;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.EditBoard
{
    public class EditBoardAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public EditBoardAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditBoardAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsHasPermissionForBoard_CanEditBoard()
        {
            var permission = BoardUserPermission.CAN_EDIT_BOARD;
            var expectedResult = true;
            var cancellationToken = new CancellationToken();
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForBoard(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == permission),
                    cancellationToken))
                .ReturnsAsync(expectedResult);

            var authorizer = new EditBoardAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new EditBoardCommand(), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForBoard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
