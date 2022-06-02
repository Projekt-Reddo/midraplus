using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace BoardService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = "";

        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        [Url]
        public string Avatar { get; set; } = null!;

        public bool IsBanned { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}