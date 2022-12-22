using ChessMobileBE.Contracts;
using ChessMobileBE.Models;
using ChessMobileBE.Models.DBModels;
using MongoDB.Driver;

namespace ChessMobileBE.Services
{
    public class RefreshTokenService: IRefreshTokenService
    {
        IMongoCollection<TokenDBModel> _collection;
        IConfiguration _config;
        public RefreshTokenService(IMongoClient client, IConfiguration config)
        {
            var database = client.GetDatabase("Tokens");
            _collection = database.GetCollection<TokenDBModel>("RefreshTokens");
            _config = config;
        }
        public TokenDBModel AddNew(NewRefreshTokenRequestModel newTokens)
        {
            TokenDBModel newRefreshToken = new TokenDBModel
            {
                AccessToken = newTokens.AcessToken,
                CreationDate = DateTime.UtcNow,
                RefreshTokenExpiryDate = DateTime.UtcNow.AddMonths(_config.GetValue<int>("Jwt:RefreshTokenExpirationInMonths")),
                AccessTokenExpiryDate = DateTime.UtcNow.AddMonths(_config.GetValue<int>("Jwt:AccessTokenExpirationInMonths")),
                IsUsed = false,
                IsRevoked = false,
                RefreshId = "",
                UserId = newTokens.UserId
            };
            _collection.InsertOne(newRefreshToken);
            return newRefreshToken;
        }
        public AuthSuccessResponse UpdateTokens(AuthSuccessResponse tokens)
        {
            // find token from db with accestoken - why?
            var tokenDatas = _collection.Find(r => r.AccessToken == tokens.Token);
            if (tokenDatas.ToList().Count > 0)
            {
                var tokendata = tokenDatas.ToList()[0];
                // delete access token from db
                _collection.DeleteOne(t => t.RefreshId == tokens.RefreshToken);
            }

            // generete new accesstoken
            var userId = Helpers.Helpers.GetUserIdFromToken(tokens.Token);
            var email = Helpers.Helpers.GetEmailFromToken(tokens.Token);
            var username = Helpers.Helpers.GetUsernameFromToken(tokens.Token);
            string newAccessToken = Helpers.Helpers.Generate(new User
            {
                Id = userId,
                Email = email,
                Username = username
            });

            // store it in db, with all datas, refresh token,...
            TokenDBModel newRefreshToken = new TokenDBModel
            {
                UserId = userId,
                IsRevoked = false,
                IsUsed = false,
                RefreshId = "",
                AccessToken = newAccessToken,
                CreationDate = DateTime.UtcNow,
                RefreshTokenExpiryDate = DateTime.UtcNow.AddMonths(_config.GetValue<int>("Jwt:RefreshTokenExpirationInMonths")),
                AccessTokenExpiryDate = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:AccessTokenExpirationInMinutes")),
            };
            _collection.InsertOne(newRefreshToken);

            AuthSuccessResponse result = new AuthSuccessResponse
            {
                RefreshToken = newRefreshToken.RefreshId,
                Token = newRefreshToken.AccessToken
            };

            // return AuthSuccessResponse result
            return result;
        }
        public DateTime? GetRefreshExpireByRefreshToken(string refreshToken)
        {
            var res = _collection.Find(t => t.RefreshId == refreshToken).ToList();
            if (res.Count > 0)
            {
                // token found
                return res[0].RefreshTokenExpiryDate;
            }
            return null;
        }
        public DateTime? GetAccessExpireByRefreshToken(string refreshToken)
        {
            var res = _collection.Find(t => t.RefreshId == refreshToken).ToList();
            if (res.Count > 0)
            {
                // token found
                return res[0].AccessTokenExpiryDate;
            }
            return null;
        }
        public void DeleteByRefreshToken(string refreshToken)
        {
            _collection.DeleteOne(t => t.RefreshId == refreshToken);
        }
        public void DeleteTokensByUserId(string userId)
        {
            _collection.DeleteMany(t => t.UserId == userId);
        }
        public void RemoveAll()
        {
            var filter = Builders<TokenDBModel>.Filter.Empty;
            _collection.DeleteMany(filter);
        }
    }
}
