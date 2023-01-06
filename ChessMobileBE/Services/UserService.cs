﻿using ChessMobileBE.Contracts;
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
                RegisterDate = DateTime.UtcNow
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

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
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
            return user;
        }
    }
}
