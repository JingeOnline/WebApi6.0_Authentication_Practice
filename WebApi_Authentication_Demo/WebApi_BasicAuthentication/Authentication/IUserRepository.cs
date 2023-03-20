namespace WebApi_BasicAuthentication.Authentication
{
    public interface IUserRepository
    {
        bool Authenticate(string username, string password);
        List<string> GetUserNames();
    }
}
