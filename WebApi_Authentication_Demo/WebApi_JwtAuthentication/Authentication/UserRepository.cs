using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_JwtAuthentication.Models;

namespace WebApi_JwtAuthentication.Authentication
{
    public class UserRepository:IUserRepository
    {
        private readonly JwtSetting _jwtSetting;

        private List<User> _users = new List<User>()
        {
            new User(){Id=1,Username="jzhang",Password="121212" },
            new User(){Id=2, Username="qxiao",Password="121212"},
        };

        public UserRepository(IOptions<JwtSetting> jwtSetting)
        {
            _jwtSetting = jwtSetting.Value;
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public AuthenticateResponseModel Authenticate(AuthenticateRequestModel request)
        {
            //验证用户名和密码是否存在且匹配
            User user = _users.FirstOrDefault(x => x.Password == request.Password && x.Username == request.Username);
            //登录成功
            if(user!=null)
            {
                string token=generateJwtToken(user);
                return new AuthenticateResponseModel(user, token);
            }
            //登录失败
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 以字符串的形式返回Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string generateJwtToken(User user)
        {
            //以字节的形式获取加密密钥
            byte[] key = Encoding.ASCII.GetBytes(_jwtSetting.SecretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                //设置ClaimsIdentity
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                //设置有效期为100秒
                Expires = DateTime.UtcNow.AddSeconds(30),
                //添加签名
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), //签名的密钥
                    SecurityAlgorithms.HmacSha256Signature  //加密算法,基于SHA-256（这里不要用Base64，Base64不使用密钥，可以直接被解码。）
                    )
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            //创建一个Json Web Token（JWT）对象
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            //返回JWT格式的字符串
            return tokenHandler.WriteToken(token);
        }
    }
}
