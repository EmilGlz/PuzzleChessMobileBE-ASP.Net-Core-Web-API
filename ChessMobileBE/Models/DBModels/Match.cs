using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChessMobileBE.Models.DBModels
{
    public class Match
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string HostId { get; set; }
        public string ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public int PuzzleIndex { get; set; }
        public List<bool> HostMoves { get; set; }
        public List<bool> ClientMoves { get; set; }
    }
}
