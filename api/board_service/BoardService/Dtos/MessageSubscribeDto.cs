namespace BoardService.Dtos
{
    public class MessageCreateBoardSubscribeDto
    {
        // user's Id
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Event { get; set; } = null!;
    }
}
