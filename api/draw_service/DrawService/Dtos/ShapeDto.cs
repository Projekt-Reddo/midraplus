namespace DrawService.Dtos
{
    public class ShapeReadDto
    {
        public string Id { get; set; } = null!;

        public string ClassName { get; set; } = null!;

        public dynamic Data { get; set; } = null!;
    }
}