public static class Constant
{
    public static class DbConfig
    {
        public const string UserId = "UserId";
        public const string Password = "Password";
    }

    public static class ShapeDataType
    {
        public const string LinePath = "LinePath";
        public const string ErasedLinePath = "ErasedLinePath";
        public const string Text = "Text";
    }
    public const string DefaultAvatarUrl = "https://i.ibb.co/kxHf8Yv/default-avatar.png";

    public static class HubReturnMethod
    {
        public const string ReceiveShape = "ReceiveShape";
        public const string ReceiveMouse = "ReceiveMouse";
        public const string OnlineUsers = "OnlineUsers";
        public const string ReceiveMessage = "ReceiveMessage";
        public const string ReceiveNewNote = "ReceiveNewNote";
        public const string ReceiveUpdateNote = "ReceiveUpdateNote";
        public const string ReceiveDeleteNote = "ReceiveDeleteNote";
        public const string LoadNotes = "LoadNotes";
        public const string ClearAll = "ClearAll";
        public const string ReceiveUndo = "ReceiveUndo";
        public const string ReceiveRedo = "ReceiveRedo";
    }

    public enum AuthResourceType
    {
        Board,
        User
    }

    public static class SystemAuthority
    {
        public const string ADMIN = "Admin";
        public const string USER = "User";
    }
}