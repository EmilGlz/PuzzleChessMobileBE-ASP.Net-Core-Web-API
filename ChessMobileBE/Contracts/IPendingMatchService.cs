using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Contracts
{
    public interface IPendingMatchService
    {
        PendingMatch GetEmptyMatch();
        PendingMatch AddNewEmptyMatch(string userId);
        void DeleteMatch(string id);
    }
}
