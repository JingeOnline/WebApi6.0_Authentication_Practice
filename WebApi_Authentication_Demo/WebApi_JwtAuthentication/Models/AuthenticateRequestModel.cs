using System.ComponentModel.DataAnnotations;

namespace WebApi_JwtAuthentication.Models
{
    public class AuthenticateRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
