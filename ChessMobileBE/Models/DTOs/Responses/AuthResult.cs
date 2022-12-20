using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Models.DTOs.Responses
{
    public class AuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public User User { get; set; }
    }
}
