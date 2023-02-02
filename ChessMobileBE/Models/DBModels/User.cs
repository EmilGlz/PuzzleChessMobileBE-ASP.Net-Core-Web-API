using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChessMobileBE.Models.DBModels
{
    public class User
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PlayGamesId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string LastMatchId { get; set; }
        public DateTime RegisterDate { get; set; }
        public int MatchCount { get; set; }
        public int VictoryCount { get; set; }
        public int DrawCount { get; set; }
        public int DefeatCount { get; set; }
        public int MateInOneLevel { get; set; }
        public int MateInTwoLevel { get; set; }
        public int MateInThreeLevel { get; set; }
        public int MateInFourLevel { get; set; }
        public int MateInFiveLevel { get; set; }
        public int GMPlaysLevel { get; set; }
        public bool GMPuzzles { get; set; }
        public bool AddRemove { get; set; }
        public Energy EnergyForOneDay { get; set; }
    }

    public class Energy
    {
        public bool IsBought { get; set; }
        public DateTime BuyTime { get; set; }
    }
}
