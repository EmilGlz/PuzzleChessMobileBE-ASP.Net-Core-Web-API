using ChessMobileBE.Helpers;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;

namespace ChessMobileBE.Contracts
{
    public interface IUserService
    {
        AuthResult Add(UserDTO dto);
        User AddMatchCount(string userId);
        User AddMatchWinState(WinState winState, string userId);
        User GetById(string id);
        User GetByPGId(string playGamesId);
        int GetMyRank(string userId);
        List<User> GetTopRankedUsers(int count);
        AuthResult Login(string playGamesId);
        void RemoveAll();
    }
}
