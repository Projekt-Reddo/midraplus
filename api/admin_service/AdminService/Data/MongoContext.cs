using MongoDB.Driver;

namespace AdminService.Data
{
    /// <summary>
    /// MongoDbContext class which present an connection to MongoDb
    /// </summary>
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        public MongoContext(MongoDbSetting mongoDbSetting)
        {
            var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);
            Database = mongoClient.GetDatabase(mongoDbSetting.DatabaseName);
        }
    }
}