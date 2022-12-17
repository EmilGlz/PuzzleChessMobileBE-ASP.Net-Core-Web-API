namespace ChessMobileBE.Models.DTOs.SignalRModels
{
    public class MoveModel
    {
        public string MoverId { get; set; }
        public bool CorrectMove { get; set; }
        public string OpponentId { get; set; }
    }
}
