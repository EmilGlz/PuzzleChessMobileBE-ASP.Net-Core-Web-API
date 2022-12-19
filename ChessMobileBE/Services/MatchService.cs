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

        public Match AddMove(string userId, string roomId, bool correctMove)
        {
            var room = Get(roomId);
            bool imHost = (room.HostId == userId) && !string.IsNullOrEmpty(userId);
            if (imHost)
            {
                var filter = Builders<Match>.Filter.Eq(r => r.Id, roomId);
                var update = Builders<Match>.Update.Push(r => r.HostMoves, correctMove);
                _collection.FindOneAndUpdate(filter, update);
                room.HostMoves.Add(correctMove);
            }
            else
            {
                var filter = Builders<Match>.Filter.Eq(r => r.Id, roomId);
                var update = Builders<Match>.Update.Push(r => r.ClientMoves, correctMove);
                _collection.FindOneAndUpdate(filter, update);
                room.ClientMoves.Add(correctMove);
            }
            return room;
        }

        public Match Get(string Id)
        {
            var gettingRoom = _collection.Find(r => r.Id == Id).ToList();
            if (gettingRoom.Count == 0)
            {
                return null;
            }
            return gettingRoom[0];
        }
    }
}
