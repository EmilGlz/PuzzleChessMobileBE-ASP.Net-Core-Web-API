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
    }
}
