using MongoDB.Bson.Serialization.Attributes;

namespace DrawService.Models
{
    public class Board
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastEdit { get; set; }

        public string Name { get; set; } = null!;

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string UserId { get; set; } = null!;

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string ChatRoomId { get; set; } = null!;

        public ICollection<Shape> Shapes { get; set; } = null!;

        public ICollection<Note> Notes { get; set; } = null!;
    }
}