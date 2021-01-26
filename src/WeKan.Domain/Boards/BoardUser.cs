using System;
using WeKan.Domain.Common.Models;

namespace WeKan.Domain.Boards
{
    public class BoardUser : Entity
    {
        internal BoardUser(int boardId, string userId, BoardUserType type)
        {
            if (boardId <= 0) throw new ArgumentOutOfRangeException(nameof(boardId));
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(nameof(userId));
            if (!Enum.IsDefined(typeof(BoardUserType), type)) throw new InvalidOperationException(nameof(type));

            BoardId = boardId;
            UserId = userId;
            Type = type;
        }

        private BoardUser() { }

        public int BoardId { get; private set; }
        public string UserId { get; private set; }
        public BoardUserType Type { get; private set; }

        public void ChangeType(BoardUserType newType)
        {
            if (!Enum.IsDefined(typeof(BoardUserType), newType)) throw new InvalidOperationException(nameof(newType));

            Type = newType;
        }
    }
}
