namespace WeKan.Domain.Boards
{
    public interface IBoardUserFactory
    {
        BoardUser CreateOwner(int boardId, string userId);
        BoardUser CreateCollaborator(int boardId, string userId);
    }

    public class BoardUserFactory : IBoardUserFactory
    {
        public BoardUser CreateCollaborator(int boardId, string userId) => new BoardUser(boardId, userId, BoardUserType.Collaborator);

        public BoardUser CreateOwner(int boardId, string userId) => new BoardUser(boardId, userId, BoardUserType.Owner);
    }
}
