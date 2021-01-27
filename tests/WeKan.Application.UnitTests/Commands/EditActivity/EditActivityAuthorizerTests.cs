using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.EditActivity;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.EditActivity
{
    public class EditActivityAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public EditActivityAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditActivityAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CallsHasPermissionForActivity_CanEditActivity()
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

            var authorizer = new EditActivityAuthorizer(_currentUserPermissionService.Object);

            var result = await authorizer.Authorise(new EditActivityCommand(), cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForActivity(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
