using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Interfaces;
using WeKan.Application.Queries.GetBoard;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Queries.GetBoard
{
    public class GetBoardAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public GetBoardAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetBoardAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsHasPermissionForBoard_CanViewBoard()
        {
            var permission = BoardUserPermission.CAN_VIEW_BOARD;
            var expectedResult = true;
            var cancellationToken = new CancellationToken();
            _currentUserPermissionService
                .Setup(s => s.HasPermissionForBoard(
                    It.IsAny<int>(),
                    It.Is<BoardUserPermission>(p => p == permission),
                    cancellationToken))
                .ReturnsAsync(expectedResult);

            var authorizer = new GetBoardAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new GetBoardQuery(1), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForBoard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
