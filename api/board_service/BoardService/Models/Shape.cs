using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BoardService.Models
{
    /// <summary>
    /// Shapes of a board
    /// </summary>
    public class Shape
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string ClassName { get; set; } = null!;

        public dynamic Data { get; set; } = null!;
    }

    /// <summary>
    /// One present type of Dynamic data in Shape. Can be used as LinePathData or EraseLinePathData
    /// </summary>
    public class PathData
    {
        public int Order { get; set; } = 0;

        public string PointColor { get; set; } = null!;

        public float[][] PointCoordinatePairs { get; set; } = null!;

        public int PointSize { get; set; }

        public bool smooth { get; set; }

        public float[][] SmoothedPointCoordinatePairs { get; set; } = null!;

        public int TailSize { get; set; } = 0;
    }

    /// <summary>
    /// One present type of Dynamic data in Shape. Can be used to store text
    /// </summary>
    public class TextData
    {
        public string Color { get; set; } = null!;

        public string Font { get; set; } = null!;

        public int ForceHeight { get; set; } = 0;

        public int ForceWidth { get; set; } = 0;

        public string Text { get; set; } = null!;

        public int V { get; set; }

        public float X { get; set; } = 0f;

        public float Y { get; set; } = 0f;
    }
}