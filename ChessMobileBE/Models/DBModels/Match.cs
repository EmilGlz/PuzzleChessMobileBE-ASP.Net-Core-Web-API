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
        public List<int> PuzzleIndexes { get; set; }
        public List<Move> HostMoves { get; set; }
        public List<Move> ClientMoves { get; set; }
        public int MatchTimeInSeconds { get; set; }
        public bool HostExited { get; set; }
        public bool ClientExited { get; set; }
    }
}
