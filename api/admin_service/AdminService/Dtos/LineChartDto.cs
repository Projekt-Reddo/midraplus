namespace AdminService.Dtos
{
    public class LineChartDto
    {
        public string Id { get; set; } = null!;
        public string Color { get; set; } = null!;
        public IEnumerable<LineData> Data { get; set; } = null!;
    }
    public class LineData
    {
        public string x { get; set; } = null!;
        public int y { get; set; }
    }
}