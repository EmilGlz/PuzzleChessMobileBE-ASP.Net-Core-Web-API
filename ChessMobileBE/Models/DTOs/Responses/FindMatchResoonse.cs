using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Models.DTOs.Responses
{
    public class FindMatchResponse
    {
        public bool IsPending { get; set; }
        public Match MatchModel { get; set; }
        public PendingMatch PendingMatchModel { get; set; }
    }
}
