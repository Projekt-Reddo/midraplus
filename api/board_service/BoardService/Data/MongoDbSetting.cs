namespace BoardService.Data
{
    /// <summary>
    /// Database configuration class
    /// </summary>
    public class MongoDbSetting
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}