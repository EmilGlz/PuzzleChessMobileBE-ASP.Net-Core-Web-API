namespace ChessMobileBE.Models.DTOs.Requests
{
    public class AddMoveDTO
    {
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public bool CorrectMove { get; set; }
        public string MoveData { get; set; }
    }
}
