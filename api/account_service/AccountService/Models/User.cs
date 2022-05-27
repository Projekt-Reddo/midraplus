using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Models
{
    /// <summary>
    /// User model which represents a user in the system.
    /// </summary>
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastEdit { get; set; }

        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        [Url]
        public string Avatar { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public bool IsBanned { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}