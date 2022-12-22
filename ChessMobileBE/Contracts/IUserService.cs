using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;

namespace ChessMobileBE.Contracts
{
    public interface IUserService
    {
        AuthResult Add(UserDTO dto);
        AuthResult Get(string playGamesId);
    }
}
