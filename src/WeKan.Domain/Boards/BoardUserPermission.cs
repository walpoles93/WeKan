namespace WeKan.Domain.Boards
{
    public enum BoardUserPermission
    {
        // Boards (101 - 200)
        CAN_VIEW_BOARD = 101,
        CAN_EDIT_BOARD = 102,
        CAN_DELETE_BOARD = 103,

        // Cards (201 - 300)
        CAN_VIEW_CARD = 201,
        CAN_EDIT_CARD = 202,
        CAN_DELETE_CARD = 203,
        CAN_CREATE_CARD = 204,

        // Activities (301 - 400)
        CAN_VIEW_ACTIVITY = 301,
        CAN_EDIT_ACTIVITY = 302,
        CAN_DELETE_ACTIVITY = 303,
        CAN_CREATE_ACTIVITY = 304,
    }
}
