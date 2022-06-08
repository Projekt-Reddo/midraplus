using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DrawService.Models
{
    public class Note
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public string Text { get; set; } = null!;
    }
}