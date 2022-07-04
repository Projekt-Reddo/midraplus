namespace AccountService.Dtos
{
    public class MessageCreateBoardPublishDto
    {
        // user's Id
        public string Id { get; set; } = null!;

        // name of the board
        public string Name { get; set; } = null!;
        public string Event { get; set; } = null!;

    }

    public class MessageAddSiginPublishDto
    {
        public string Event { get; set; } = null!;
    }
}