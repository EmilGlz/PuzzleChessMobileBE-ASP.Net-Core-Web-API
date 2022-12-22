using ChessMobileBE.Models.DBModels;

namespace ChessMobileBE.Models.DTOs.Responses
{
    public class AuthResult
    {
        public string AccessToken { get; set; }
        public User User { get; set; }
    }
}
