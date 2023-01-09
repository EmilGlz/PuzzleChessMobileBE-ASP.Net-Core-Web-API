namespace ChessMobileBE.Models.DTOs.Responses
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string PlayGamesId { get; set; }
        public string Username { get; set; }
        public int Rating { get; set; }
        public int VictoryCount { get; set; }
        public int DrawCount { get; set; }
        public int DefeatCount { get; set; }
    }
}
