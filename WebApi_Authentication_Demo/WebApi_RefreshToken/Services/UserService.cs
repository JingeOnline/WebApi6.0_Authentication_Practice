using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi_RefreshToken.Authorization;
using WebApi_RefreshToken.Helpers;
using WebApi_RefreshToken.Models;

namespace WebApi_RefreshToken.Services
{
    public class UserService : IUserService
    {
        private DataContext _context;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public UserService(DataContext context,IJwtUtils jwtUtils,IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _context.Users.Include(x=>x.RefreshTokens).SingleOrDefault(x => x.Username == model.Username);

            // validate
            //if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            if (user == null || Sha256Helper.VerifySha256(model.Password, user.PasswordHash))
            {
                throw new AppException("Username or password is incorrect");
            }

            //Revoke old RefreshToken
            IEnumerable<RefreshToken> oldRefreshTokens = user.RefreshTokens.Where(x => x.IsActive == true);
            foreach(RefreshToken rft in oldRefreshTokens)
            {
                setRevokeRefreshToken(rft,ipAddress,"User login.");
            }


            // authentication successful so generate jwt and refresh tokens
            string jwtToken = _jwtUtils.GenerateJwtToken(user);
            RefreshToken refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);


            // save changes to db
            _context.Update(user);
            _context.SaveChanges();

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        /// <summary>
        /// 传入RefreshToken，获取新的JWT Token。
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token, the token already expired or revoked.");

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        /// <summary>
        /// 传入RefreshToken字符串，禁用该RefreshToken。
        /// 禁用RefreshToken后，用户需要输入用户名和密码，再次登录。这里并没有禁止用户登录获取新的RefreshToken。
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <exception cref="AppException"></exception>
        public void RevokeToken(string token, string ipAddress)
        {
            //var user = getUserByRefreshToken(token);
            //var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            RefreshToken refreshToken = _context.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                throw new AppException("Invalid token, the token already expired or revoked.");
            }
            setRevokeRefreshToken(refreshToken, ipAddress, "Revoked by call the revoke api.");
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        // helper methods

        /// <summary>
        /// 根据RefreshToken字符串查找User
        /// </summary>
        /// <param name="token">RefreshToken的字符串</param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        private User getUserByRefreshToken(string token)
        {
            //var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            RefreshToken refreshToken = _context.RefreshTokens.Include(x => x.User).Single(x => x.Token == token);

            if (refreshToken == null)
                throw new AppException("Invalid token, cannot find this refresh token in database.");

            return refreshToken.User;
        }

        /// <summary>
        /// 更新被禁用的RefreshToken
        /// </summary>
        /// <param name="token">被禁用的RefreshToken对象</param>
        /// <param name="ipAddress">当前用户请求的IP地址</param>
        /// <param name="reason">禁用该Token的原因</param>
        private void setRevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null)
        {
            token.RevokeAt = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
        }
    }
}
