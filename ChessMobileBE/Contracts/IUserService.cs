using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;

namespace ChessMobileBE.Contracts
{
    public interface IUserService
    {
        User Add(UserDTO dto);
        User Get(string playGamesId);
    }
}
