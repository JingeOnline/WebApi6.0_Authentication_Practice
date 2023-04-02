using System.ComponentModel.DataAnnotations;

namespace WebApi_JwtAuthentication.Models
{
    /// <summary>
    /// 用户发送登录请求时的Model
    /// </summary>
    public class AuthenticateRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
