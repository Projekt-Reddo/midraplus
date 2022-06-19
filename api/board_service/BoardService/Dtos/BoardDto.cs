namespace BoardService.Dtos
{
    public class BoardReadDto
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime LastEdit { get; set; }
    }

    public class BoardCreateDto
    {
        public string UserId { get; set; } = null!;
    }

    public class BoardUpdateDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}