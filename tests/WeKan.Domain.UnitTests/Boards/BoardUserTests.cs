using System;
using WeKan.Domain.Boards;
using Xunit;

namespace WeKan.Domain.UnitTests.Boards
{
    public class BoardUserTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Ctor_BoardIdLessThanEqualZero_ThrowsArgumentOutOfRangeException(int boardId)
        {
            var userId = "user-id";
            var type = BoardUserType.Owner;

            BoardUser action() => new BoardUser(boardId, userId, type);

            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_UserId_NullOrEmpty_ThrowsArgumentException(string userId)
        {
            var boardId = 1;
            var type = BoardUserType.Owner;

            BoardUser action() => new BoardUser(boardId, userId, type);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Ctor_InvalidBoardUserType_ThrowsInvalidOperationException()
        {
            var boardId = 1;
            var userId = "user-id";
            var type = (BoardUserType)1000;

            BoardUser action() => new BoardUser(boardId, userId, type);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void Ctor_CreateBoardUserWithExpectedPropertyValues()
        {
            var boardId = 1;
            var userId = "user-id";
            var type = BoardUserType.Owner;

            var boardUser = new BoardUser(boardId, userId, type);

            Assert.Equal(boardId, boardUser.BoardId);
            Assert.Equal(userId, boardUser.UserId);
            Assert.Equal(type, boardUser.Type);
        }

        [Fact]
        public void ChangeType_InvalidBoardUserType_ThrowsInvalidOperationException()
        {
            var boardId = 1;
            var userId = "user-id";
            var type = BoardUserType.Owner;
            var boardUser = new BoardUser(boardId, userId, type);

            void action() => boardUser.ChangeType((BoardUserType)1000);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void ChangeType_UpdatesTypeValue()
        {
            var boardId = 1;
            var userId = "user-id";
            var type = BoardUserType.Owner;
            var newType = BoardUserType.Collaborator;
            var boardUser = new BoardUser(boardId, userId, type);

            boardUser.ChangeType(newType);

            Assert.Equal(newType, boardUser.Type);
        }
    }
}
