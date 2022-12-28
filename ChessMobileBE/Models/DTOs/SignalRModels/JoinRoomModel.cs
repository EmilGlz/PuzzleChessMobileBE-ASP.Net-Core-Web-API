namespace ChessMobileBE.Models.DTOs.SignalRModels
{
    public class JoinRoomModel
    {
        public string CliendId { get; set; }
        public string HostId { get; set; }
        public string RoomId { get; set; }
    }
}
