namespace WebApi_JwtAuthentication.Models
{
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
