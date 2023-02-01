using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;

namespace ChessMobileBE.Contracts
{
    public interface IMatchService
    {
        Match Add(PendingMatch pendingMatch, string clientId);
        Match AddMove(AddMoveDTO model);
        void Delete(string Id);
        Match Get(string Id);
        List<Match> GetRoomsByUserId(string userId);
        void LoseAllRooms(string userId);
        void RemoveAll();
    }
}
