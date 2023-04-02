using WebApi_JwtAuthentication.Models;

namespace WebApi_JwtAuthentication.Authentication
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        AuthenticateResponseModel Authenticate(AuthenticateRequestModel request);
    }
}
