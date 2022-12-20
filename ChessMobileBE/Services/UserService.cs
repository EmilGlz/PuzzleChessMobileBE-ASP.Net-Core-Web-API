using ChessMobileBE.Contracts;
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
        
        public User Get(string playGamesId)
        {
            var gettingUser = _collection.Find(u => u.PlayGamesId == playGamesId).ToList();
            if (gettingUser.Count > 0)
                return gettingUser[0];
            else
                return null;
        }

        public User Add(UserDTO dto)
        {
            var newUser = new User
            {
                Id = "",
                Email = dto.Email,
                PlayGamesId = dto.PlayGamesId,
                Username = dto.Username,
                RegisterDate = DateTime.UtcNow
            };
            _collection.InsertOne(newUser);
            return newUser;
        }

    }
}
