using ChessMobileBE.Contracts;
using ChessMobileBE.Helpers;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;
using MongoDB.Driver;

namespace ChessMobileBE.Services
{
    public class UserService : IUserService
    {
        private IMongoCollection<User> _collection;

        public UserService(IMongoClient client)
        {
            var database = client.GetDatabase("Users");
            _collection = database.GetCollection<User>("UsersCollection");
        }

        public int GetMyRank(string userId)
        {
            var sortedUsers = _collection.Find(_ => true).ToList().OrderBy(u => u.VictoryCount).Reverse().ToList();
            var myRank = sortedUsers.FindIndex(u => u.Id == userId);
            return myRank;
        }

        public List<User> GetTopRankedUsers(int count)
        {
            var sortedUsers = _collection.Find(_ => true).ToList().OrderBy(u => u.VictoryCount).Reverse().ToList();
            if (sortedUsers.Count < count)
            {
                return sortedUsers;
            }
            var res = sortedUsers.GetRange(sortedUsers.Count - count, sortedUsers.Count);
            return res;
        }

        public AuthResult Login(string playGamesId)
        {
            var gettingUser = _collection.Find(u => u.PlayGamesId == playGamesId).ToList();
            if (gettingUser.Count > 0)
            {
                var token = Helpers.Helpers.Generate(gettingUser[0]);
                return new AuthResult
                {
                    User = gettingUser[0],
                    AccessToken = token
                };
            }
            else
                return null;
        }

        public User GetByPGId(string playGamesId)
        {
            var gettingUser = _collection.Find(u => u.PlayGamesId == playGamesId).ToList();
            if (gettingUser.Count == 0)
                return null;
            return gettingUser[0];
        }

        public User GetById(string id)
        {
            var gettingUser = _collection.Find(u => u.Id == id).ToList();
            if (gettingUser.Count == 0)
                return null;
            return gettingUser[0];
        }

        public AuthResult Add(UserDTO dto)
        {
            var newUser = new User
            {
                Id = "",
                Email = dto.Email,
                PlayGamesId = dto.PlayGamesId,
                Username = dto.Username,
                RegisterDate = DateTime.UtcNow,
                DefeatCount = 0,
                DrawCount = 0,
                VictoryCount = 0,
                MateInOneLevel = 1,
                MateInTwoLevel = 1,
                MateInThreeLevel = 0,
                MateInFourLevel = 0,
                MateInFiveLevel = 0,
                GMPlaysLevel = 0,
            };
            _collection.InsertOne(newUser);
            var token = Helpers.Helpers.Generate(newUser);
            return new AuthResult
            {
                User = newUser,
                AccessToken = token
            };
        }

        public User AddMatchWinState(WinState winState, string userId)
        {
            var user = GetById(userId);
            user.MatchCount++;
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var matchCountUpdate = Builders<User>.Update.Set(r => r.MatchCount, user.MatchCount);
            if (winState == WinState.Win)
            {
                user.VictoryCount ++;
                var update = Builders<User>.Update.Set(r => r.VictoryCount, user.VictoryCount);
                _collection.FindOneAndUpdate(filter, update);
            }
            else if (winState == WinState.Draw)
            {
                user.DrawCount++;
                var update = Builders<User>.Update.Set(r => r.DrawCount, user.DrawCount);
                _collection.FindOneAndUpdate(filter, update);
            }
            else if (winState == WinState.Lose)
            {
                user.DefeatCount++;
                var update = Builders<User>.Update.Set(r => r.DefeatCount, user.DefeatCount);
                _collection.FindOneAndUpdate(filter, update);
            }
            _collection.FindOneAndUpdate(filter, matchCountUpdate);
            return user;
        }

        public User AddMatchCount(string userId)
        {
            var user = GetById(userId);
            if (user == null)
                return null;
            user.MatchCount++;
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(u => u.MatchCount, user.MatchCount);
            _collection.UpdateOne(filter, update);
            return user;
        }

        public void RemoveAll()
        {
            var filter = Builders<User>.Filter.Empty;
            _collection.DeleteMany(filter);
        }
    }
}
