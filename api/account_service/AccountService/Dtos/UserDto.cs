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
}