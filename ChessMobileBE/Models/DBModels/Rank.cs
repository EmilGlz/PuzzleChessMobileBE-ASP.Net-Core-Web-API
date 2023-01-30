using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChessMobileBE.Models.DBModels
{
    public class Rank
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Value { get; set; }
    }
}
