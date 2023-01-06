namespace ChessMobileBE.Models.DTOs.Responses
{
    public class FinishMatchResponse
    {
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string OpponendId { get; set; }
        public bool Victory { get; set; }
        public int UserCorrectMoveCount { get; set; }
        public int OpponentCorrectMoveCount { get; set; }
    }
}
