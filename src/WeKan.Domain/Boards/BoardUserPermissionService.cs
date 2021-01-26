using System;

namespace WeKan.Domain.Boards
{
    public interface IBoardUserPermissionService
    {
        bool HasPermission(BoardUser boardUser, BoardUserPermission permission);
    }

    public class BoardUserPermissionService : IBoardUserPermissionService
    {
        public bool HasPermission(BoardUser boardUser, BoardUserPermission permission)
        {
            if (boardUser is null) throw new ArgumentNullException(nameof(boardUser));
            if (!Enum.IsDefined(typeof(BoardUserPermission), permission)) throw new InvalidOperationException(nameof(permission));

            bool CollaboratorHasPermission() => permission switch
            {
                BoardUserPermission.CAN_EDIT_BOARD => false,
                BoardUserPermission.CAN_DELETE_BOARD => false,
                _ => true
            };

            return boardUser.Type switch
            {
                BoardUserType.None => false,
                BoardUserType.Owner => true,
                BoardUserType.Collaborator => CollaboratorHasPermission(),
                _ => throw new InvalidOperationException(nameof(boardUser.Type))
            };
        }
    }
}
