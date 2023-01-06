using ChessMobileBE.Helpers;
using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Models.DTOs.Responses
{
    public class FinishMatchResponse
    {
        public string RoomId { get; set; }
        public WinState VictoryState { get; set; }
        public int UserCorrectMoveCount { get; set; }
        public int OpponentCorrectMoveCount { get; set; }
        public User UserModel { get; set; }
    }
}
