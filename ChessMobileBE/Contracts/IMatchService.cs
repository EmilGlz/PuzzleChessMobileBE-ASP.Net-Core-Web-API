using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Contracts
{
    public interface IMatchService
    {
        Match Add(string hostId, string clientId, int puzzleIndex);
        Match AddMove(string userId, string roomId, bool correctMove);
        Match Get(string Id);
    }
}
