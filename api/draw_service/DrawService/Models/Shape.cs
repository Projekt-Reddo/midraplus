using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DrawService.Models
{
    [BsonIgnoreExtraElements]
    public class Shape
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string ClassName { get; set; } = null!;
        public dynamic Data { get; set; } = null!;
    }
}