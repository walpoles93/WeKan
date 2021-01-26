using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Domain.UnitTests.Boards
{
    public class BoardUserFactoryTests
    {
        [Fact]
        public void CreateOwner_ReturnsBoardUserWithOwnerType()
        {
            var boardId = 1;
            var userId = "user-id";
            var factory = new BoardUserFactory();

            var boardUser = factory.CreateOwner(boardId, userId);

            Assert.Equal(boardId, boardUser.BoardId);
            Assert.Equal(userId, boardUser.UserId);
            Assert.Equal(BoardUserType.Owner, boardUser.Type);
        }

        [Fact]
        public void CreateCollaborator_ReturnsBoardUserWithCollaboratorType()
        {
            var boardId = 1;
            var userId = "user-id";
            var factory = new BoardUserFactory();

            var boardUser = factory.CreateCollaborator(boardId, userId);

            Assert.Equal(boardId, boardUser.BoardId);
            Assert.Equal(userId, boardUser.UserId);
            Assert.Equal(BoardUserType.Collaborator, boardUser.Type);
        }
    }
}
