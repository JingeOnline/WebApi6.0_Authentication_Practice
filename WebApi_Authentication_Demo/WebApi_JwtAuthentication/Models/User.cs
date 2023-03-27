using System.Text.Json.Serialization;

namespace WebApi_JwtAuthentication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        
        [JsonIgnore]
        public string Password { get; set; }
    }
}
