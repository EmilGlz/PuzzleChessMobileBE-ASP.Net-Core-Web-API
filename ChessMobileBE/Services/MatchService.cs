using ChessMobileBE.Contracts;
using ChessMobileBE.Models.DBModels;
using MongoDB.Driver;

namespace ChessMobileBE.Services
{
    public class MatchService : IMatchService
    {
        private IMongoCollection<Match> _collection;

        public MatchService(IMongoClient client)
        {
            var database = client.GetDatabase("Matches");
            _collection = database.GetCollection<Match>("MatchesCollection");
        }
        public Match Add(string hostId, string clientId, int puzzleIndex)
        {
            var dbModel = new Match
            {
                Id = "",
                HostId = hostId,
                ClientId = clientId,
                StartDate = DateTime.UtcNow,
                PuzzleIndex = puzzleIndex
            };
            _collection.InsertOne(dbModel);
            return dbModel;
        }
    }
}
