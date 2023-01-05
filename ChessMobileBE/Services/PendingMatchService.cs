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
                PuzzleIndexes = GenerateRandomList(10)
            };
            _collection.InsertOne(dbModel);
            return dbModel;
        }

        public void Delete(string id)
        {
            _collection.DeleteOne(u => u.Id == id);
        }

        public PendingMatch GetEmptyMatch(string userId)
        {
            var emptyMatch = _collection.Find(pm => !string.IsNullOrEmpty(pm.Id) && pm.UserId != userId).ToList();
            if (emptyMatch.Count == 0)
                return null;
            return emptyMatch[0];
        }

        List<int> GenerateRandomList(int count)
        {
            Random random = new Random();
            List<int> values = new List<int>();
            for (int i = 0; i < count; ++i)
                values.Add(random.Next() % 1000);
            return values;
        }
    }
}
