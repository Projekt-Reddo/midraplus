namespace AdminService.Dtos
{
    public class BoardLoadByTime
    {
        public string Id { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}