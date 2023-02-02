using ChessMobileBE.Contracts;
using ChessMobileBE.Models;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
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

        public Match Add(PendingMatch pendingMatch, string clientId)
        {
            var dbModel = new Match
            {
                Id = "",
                HostId = pendingMatch.UserId,
                ClientId = clientId,
                StartDate = DateTime.UtcNow,
                PuzzleIndexes = pendingMatch.PuzzleIndexes,
                ClientMoves = new List<Move>(),
                HostMoves = new List<Move>(),
                MatchTimeInSeconds = int.Parse(MyConfigurationManager.AppSetting.GetSection("MatchTimeInSeconds").Value)
            };
            _collection.InsertOne(dbModel);
            return dbModel;
        }

        public Match AddMove(AddMoveDTO model)
        {
            var room = Get(model.RoomId);
            bool imHost = (room.HostId == model.UserId) && !string.IsNullOrEmpty(model.UserId);
            var pushingObj = new Move
            {
                CorrectMove = model.CorrectMove,
                Date = DateTime.UtcNow,
                MoveData = model.MoveData
            };
            var filter = Builders<Match>.Filter.Eq(r => r.Id, model.RoomId);
            if (imHost)
            {
                var update = Builders<Match>.Update.Push(r => r.HostMoves, pushingObj);
                _collection.FindOneAndUpdate(filter, update);
                room.HostMoves.Add(pushingObj);
            }
            else
            {
                var update = Builders<Match>.Update.Push(r => r.ClientMoves, pushingObj);
                _collection.FindOneAndUpdate(filter, update);
                room.ClientMoves.Add(pushingObj);
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

        public List<Match> GetRoomsByUserId(string userId)
        {
            var res = new List<Match>();
            var gettingRoom = _collection.Find(r => r.HostId == userId).ToList();
            var gettingRoom1 = _collection.Find(r => r.ClientId == userId).ToList();
            res.AddRange(gettingRoom);
            res.AddRange(gettingRoom1);
            return res;
        }

        public void Delete(string Id)
        {
            _collection.DeleteOne(m => m.Id == Id);
        }

        public void LoseAllRooms(string userId)
        {
            var rooms = GetRoomsByUserId(userId);
            for (int i = 0; i < rooms.Count; i++)
            {
                LoseInOneRoom(userId, rooms[i]);
            }
        }

        public void LoseInOneRoom(string userId, Match room)
        {
            var filter = Builders<Match>.Filter.Eq(m => m.Id, room.Id);
            if (Helpers.Helpers.IsHostInCurrentOnlineMatch(room, userId).Value)
            {
                var update = Builders<Match>.Update.Set(r => r.HostExited, true);
                _collection.FindOneAndUpdate(filter, update);
            }
            else
            {
                var update = Builders<Match>.Update.Set(r => r.ClientExited, true);
                _collection.FindOneAndUpdate(filter, update);
            }
        }

        public Match GiveUp(string userId, string roomId)
        {
            var room = Get(roomId);
            var filter = Builders<Match>.Filter.Eq(m => m.Id, roomId);
            if (Helpers.Helpers.IsHostInCurrentOnlineMatch(room, userId).Value)
            {
                room.HostExited = true;
                var update = Builders<Match>.Update.Set(r => r.HostExited, true);
                _collection.FindOneAndUpdate(filter, update);
            }
            else
            {
                room.ClientExited = true;
                var update = Builders<Match>.Update.Set(r => r.ClientExited, true);
                _collection.FindOneAndUpdate(filter, update);
            }
            return room;
        }

        public void RemoveAll()
        {
            var filter = Builders<Match>.Filter.Empty;
            _collection.DeleteMany(filter);
        }

    }
}
