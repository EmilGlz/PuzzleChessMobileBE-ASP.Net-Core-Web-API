using ChessMobileBE.Contracts;
using ChessMobileBE.Models.DBModels;
using MongoDB.Driver;

namespace ChessMobileBE.Services
{
    public class PendingMatchService : IPendingMatchService
    {
        private IMongoCollection<PendingMatch> _collection;

        public PendingMatchService(IMongoClient client)
        {
            var database = client.GetDatabase("PendingMatches");
            _collection = database.GetCollection<PendingMatch>("PendingMatchesCollection");
        }

        public PendingMatch AddNewEmptyMatch(string userId)
        {
            var existingUser = _collection.Find(pm => pm.UserId == userId).ToList();
            if (existingUser.Count > 0)
                return null;
            Random random = new Random();
            var dbModel = new PendingMatch
            {
                UserId = userId,
                Id = "",
                StartDate = DateTime.UtcNow,
                PuzzleIndex = random.Next(0, 1000),
            };
            _collection.InsertOne(dbModel);
            return dbModel;
        }

        public void DeleteMatch(string id)
        {
            _collection.DeleteOne(u => u.Id == id);
        }

        public PendingMatch GetEmptyMatch()
        {
            var emptyMatch = _collection.Find(pm => !string.IsNullOrEmpty(pm.Id)).ToList();
            if (emptyMatch.Count == 0)
                return null;
            return emptyMatch[0];
        }
    }
}
