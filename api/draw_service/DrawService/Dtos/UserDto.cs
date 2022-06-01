namespace DrawService.Dtos
{
    /// <summary>
    /// Class store user information in draw
    /// </summary>
    public class DrawConnections
    {
        public UserConnectionInfo User { get; set; } = null!;

        public string BoardId { get; set; } = null!;
    }

    /// <summary>
    /// Store user information in connection list
    /// </summary>
    public class UserConnectionInfo
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Avatar { get; set; } = null!;
    }
}