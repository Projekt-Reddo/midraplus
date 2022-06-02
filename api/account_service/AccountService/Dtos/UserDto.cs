namespace AccountService.Dtos
{
    public class UserReadDto
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Avatar { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public bool IsBanned { get; set; } = false;
    }
    public class UserView
    {
        public string tokenId { get; set; } = null!;
    }

    public class UserConnection
    {
        public UserInfoInUserConnection User { get; set; } = null!;
        public string Board { get; set; } = null!;
    }

    public class UserConnectionChat
    {
        public UserInfoInUserConnection User { get; set; } = null!;
        public string Board { get; set; } = null!;
        public string Type = "Chat";
    }

    public class UserInfoInUserConnection
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Avatar { get; set; } = null!;
    }

    public class UserManageListDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public bool IsBanned { get; set; } = false;
    }

}