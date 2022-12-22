using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Contracts
{
    public interface IMatchService
    {
        Match Add(PendingMatch pendingMatch, string clientId);
        Match AddMove(string userId, string roomId, bool correctMove);
        void Delete(string Id);
        Match Get(string Id);
    }
}
