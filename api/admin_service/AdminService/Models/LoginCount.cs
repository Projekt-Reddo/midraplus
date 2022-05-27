using MongoDB.Bson.Serialization.Attributes;

namespace AdminService.Models
{
    public class LoginCount
    {
        [BsonId]
        public DateTime At { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        public int Times { get; set; } = 0;
    }
}