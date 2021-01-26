using System;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Domain.UnitTests.Boards
{
    public class BoardUserPermissionServiceTests
    {
        [Fact]
        public void HasPermission_BoardUserNull_ThrowsArgumentNullException()
        {
            var service = new BoardUserPermissionService();

            void action() => service.HasPermission(null, BoardUserPermission.CAN_CREATE_ACTIVITY);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void HasPermission_PermissionNotValid_ThrowsInvalidOperationException()
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.Collaborator);

            void action() => service.HasPermission(boardUser, (BoardUserPermission)100000000);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Theory]
        [InlineData(BoardUserPermission.CAN_VIEW_BOARD)]
        [InlineData(BoardUserPermission.CAN_EDIT_BOARD)]
        [InlineData(BoardUserPermission.CAN_DELETE_BOARD)]
        [InlineData(BoardUserPermission.CAN_VIEW_CARD)]
        [InlineData(BoardUserPermission.CAN_EDIT_CARD)]
        [InlineData(BoardUserPermission.CAN_DELETE_CARD)]
        [InlineData(BoardUserPermission.CAN_CREATE_CARD)]
        [InlineData(BoardUserPermission.CAN_VIEW_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_EDIT_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_DELETE_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_CREATE_ACTIVITY)]
        public void HasPermission_IsOwner_ReturnsTrue(BoardUserPermission permission)
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.Owner);

            var result = service.HasPermission(boardUser, permission);

            Assert.True(result);
        }

        [Fact]
        public void HasPermission_CanEditBoard_IsCollaborator_ReturnsFalse()
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.Collaborator);

            var result = service.HasPermission(boardUser, BoardUserPermission.CAN_EDIT_BOARD);

            Assert.False(result);
        }

        [Fact]
        public void HasPermission_CanDeleteBoard_IsCollaborator_ReturnsFalse()
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.Collaborator);

            var result = service.HasPermission(boardUser, BoardUserPermission.CAN_DELETE_BOARD);

            Assert.False(result);
        }

        [Fact]
        public void HasPermission_CanViewBoard_IsCollaborator_ReturnsTrue()
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.Collaborator);

            var result = service.HasPermission(boardUser, BoardUserPermission.CAN_VIEW_BOARD);

            Assert.True(result);
        }

        [Theory]
        [InlineData(BoardUserPermission.CAN_VIEW_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_EDIT_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_DELETE_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_CREATE_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_VIEW_CARD)]
        [InlineData(BoardUserPermission.CAN_EDIT_CARD)]
        [InlineData(BoardUserPermission.CAN_DELETE_CARD)]
        [InlineData(BoardUserPermission.CAN_CREATE_CARD)]
        public void HasPermission_AllCardAndActivityPermissions_IsCollaborator_ReturnsTrue(BoardUserPermission permission)
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.Collaborator);

            var result = service.HasPermission(boardUser, permission);

            Assert.True(result);
        }

        [Theory]
        [InlineData(BoardUserPermission.CAN_VIEW_BOARD)]
        [InlineData(BoardUserPermission.CAN_EDIT_BOARD)]
        [InlineData(BoardUserPermission.CAN_DELETE_BOARD)]
        [InlineData(BoardUserPermission.CAN_VIEW_CARD)]
        [InlineData(BoardUserPermission.CAN_EDIT_CARD)]
        [InlineData(BoardUserPermission.CAN_DELETE_CARD)]
        [InlineData(BoardUserPermission.CAN_CREATE_CARD)]
        [InlineData(BoardUserPermission.CAN_VIEW_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_EDIT_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_DELETE_ACTIVITY)]
        [InlineData(BoardUserPermission.CAN_CREATE_ACTIVITY)]
        public void HasPermission_IsNone_ReturnsFalse(BoardUserPermission permission)
        {
            var service = new BoardUserPermissionService();
            var boardUser = new BoardUser(1, "user-id", BoardUserType.None);

            var result = service.HasPermission(boardUser, permission);

            Assert.False(result);
        }
    }
}
