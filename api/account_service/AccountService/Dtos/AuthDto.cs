namespace AccountService.Dtos
{
    public class AuthReadDto
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Avatar { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public string AccessToken { get; set; } = null!; // Need specify when create

        public bool IsBanned { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}