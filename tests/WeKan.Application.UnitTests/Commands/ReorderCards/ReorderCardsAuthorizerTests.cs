using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.ReorderCards;
using WeKan.Application.Common.Interfaces;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Application.UnitTests.Commands.ReorderCards
{
    public class ReorderCardsAuthorizerTests
    {
        private readonly Mock<ICurrentUserPermissionService> _currentUserPermissionService;

        public ReorderCardsAuthorizerTests()
        {
            _currentUserPermissionService = new Mock<ICurrentUserPermissionService>();
        }

        [Fact]
        public void Ctor_ICurrentUserPermissionServiceNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ReorderCardsAuthorizer(null));
        }

        [Fact]
        public async Task Authorise_CardIdsNull_ReturnsTrue()
        {
            var authorizer = new ReorderCardsAuthorizer(_currentUserPermissionService.Object);
            var request = new ReorderCardsCommand { CardIds = null };
            var cancellationToken = new CancellationToken();

            var result = await authorizer.Authorise(request, cancellationToken);

            Assert.True(result);
        }

        [Fact]
        public async Task CardIdsEmpty_ReturnsTrue()
        {
            var authorizer = new ReorderCardsAuthorizer(_currentUserPermissionService.Object);
            var request = new ReorderCardsCommand { CardIds = new List<int>() };
            var cancellationToken = new CancellationToken();

            var result = await authorizer.Authorise(request, cancellationToken);

            Assert.True(result);
        }

        [Fact]
        public async Task Authorise_FirstCardId_CallsHasPermissionForCard_CanEditCard()
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

            var authorizer = new ReorderCardsAuthorizer(_currentUserPermissionService.Object);
            var request = new ReorderCardsCommand { CardIds = new List<int> { 1 } };
            var result = await authorizer.Authorise(request, cancellationToken);

            Assert.Equal(expectedResult, result);
            _currentUserPermissionService
                .Verify(s => s.HasPermissionForCard(It.IsAny<int>(), It.Is<BoardUserPermission>(p => p == permission), cancellationToken), Times.Once);
        }
    }
}
