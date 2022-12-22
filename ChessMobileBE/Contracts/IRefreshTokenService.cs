using ChessMobileBE.Models;

namespace ChessMobileBE.Contracts
{
    public interface IRefreshTokenService
    {
        TokenDBModel AddNew(NewRefreshTokenRequestModel newTokens);
        AuthSuccessResponse UpdateTokens(AuthSuccessResponse tokens);
        DateTime? GetRefreshExpireByRefreshToken(string refreshToken);
        DateTime? GetAccessExpireByRefreshToken(string refreshToken);
        void DeleteByRefreshToken(string refreshToken);
        void DeleteTokensByUserId(string userId);
        void RemoveAll();
    }
}
