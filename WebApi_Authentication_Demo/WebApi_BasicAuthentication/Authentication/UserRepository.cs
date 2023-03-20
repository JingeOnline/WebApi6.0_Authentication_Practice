using WebApi_BasicAuthentication.Authentication.Models;

namespace WebApi_BasicAuthentication.Authentication
{
    public class UserRepository:IUserRepository
    {
        private List<User> _users = new List<User>()
        {
            new User(){Id=1,Name="jzhang",Password="123456" },
            new User(){Id=2, Name="qxiao",Password="888888"},
        };

        public bool Authenticate(string username, string password)
        {
            if(_users.Any(x=>x.Name==username && x.Password==password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetUserNames()
        {
            return _users.Select(x=>x.Name).ToList();
        }
    }
}
