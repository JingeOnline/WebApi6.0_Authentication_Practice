namespace WebApi_JwtAuthentication.Models
{
    /// <summary>
    /// 用户登录成功时返回的Model
    /// </summary>
    public class AuthenticateResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponseModel(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}
