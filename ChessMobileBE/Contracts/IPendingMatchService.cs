using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Contracts
{
    public interface IPendingMatchService
    {
        PendingMatch GetEmptyMatch(string userId);
        PendingMatch AddNewEmptyMatch(string userId);
        void Delete(string id);
    }
}
