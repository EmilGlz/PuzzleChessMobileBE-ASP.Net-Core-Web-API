using ChessMobileBE.Models;
using ChessMobileBE.Models.DBModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChessMobileBE.Helpers
{
    public class Helpers
    {
        public static string Generate(User user)
        {
            var keyString = MyConfigurationManager.AppSetting.GetSection("Jwt").GetSection("SecretKey").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("PlayGamesId", user.PlayGamesId)

            };
            //var token = new JwtSecurityToken(_config.GetValue<string>("Jwt:Issuer"),
            var token = new JwtSecurityToken(MyConfigurationManager.AppSetting.GetSection("Jwt").GetSection("Issuer").Value,
                MyConfigurationManager.AppSetting.GetSection("Jwt").GetSection("Audience").Value,
                claims,
                expires: DateTime.UtcNow.AddMonths(int.Parse(MyConfigurationManager.AppSetting.GetSection("Jwt").GetSection("AccessTokenExpirationInMonths").Value)), //_config.GetValue<int>("Jwt:AccessTokenExpirationInMinutes")),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static string GetEmailFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        }

        public static string GetUsernameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        }

        public static bool? IsHostInCurrentOnlineMatch(Match match, string userId)
        {
            if (match.HostId == userId)
                return true;
            else if (match.ClientId == userId)
                return false;
            return null;
        }

        public static bool MatchIsFinishedByTime(Match match)
        {
            var matchTimeInSeconds = int.Parse(MyConfigurationManager.AppSetting.GetSection("MatchTimeInSeconds").Value);
            var matchDeadline = match.StartDate.AddSeconds(matchTimeInSeconds);
            if (matchDeadline <= DateTime.UtcNow)
                return true;
            return false;
        }

        public static int HostCorrectMoveCount(Match match)
        {
            return match.HostMoves.FindAll(m => m.CorrectMove).Count;
        }

        public static int ClientCorrectMoveCount(Match match)
        {
            return match.ClientMoves.FindAll(m => m.CorrectMove).Count;
        }

        public static string OppIdOfRoom(Match match, string myUserId)
        {
            if (match.HostId == myUserId)
                return match.ClientId;
            else if (match.ClientId == myUserId)
                return match.HostId;
            return "";
        }
    }

    public enum WinState
    {
        None = 0,
        Win = 1,
        Lose = 2,
        Draw = 3
    }
}
