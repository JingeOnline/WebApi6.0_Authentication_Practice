using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using WebApi_RefreshToken.Helpers;

namespace WebApi_RefreshToken.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public int ExpiredInMinutes { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        //private AppSettings _appSettings;

        public AuthenticateResponse(User user, string jwtToken, string refreshToken, AppSettings appSettings)
        {
            Id = user.Id;
            Username = user.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            ExpiredInMinutes = appSettings.JwtTokenTTL_Minutes;
        }
    }
}
