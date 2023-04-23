using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi_RefreshToken.Helpers;
using WebApi_RefreshToken.Models;
using WebApi_RefreshToken.Services;

namespace WebApi_RefreshToken.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public JwtUtils(DataContext context,IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 1 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtTokenTTL_Minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = getUniqueToken(),
                //Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpireAt = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenTTL_Days),
                CreateAt = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            Debug.WriteLine($"RefreshToken={refreshToken.Token}");
            return refreshToken;

            string getUniqueToken()
            {
                // token is a cryptographically strong random sequence of values
                byte[] bytes = RandomNumberGenerator.GetBytes(64);

                //这里不要使用Base64,因为普通的Base64包含'/','='等特殊符号，在储存到Cookie中时，会被进行URL编码，变成%4%等字符。
                //之后在匹配的时候，会造成cookie中的Tooken值和数据库中的Tooken值不一致。
                //使用Base64UrlEncoder编码和解码就能避免出现这些特殊字符。
                string token = Base64UrlEncoder.Encode(bytes);
                //string token=Convert.ToBase64String(bytes); //不要使用

                return token;
            }
        }
    }
}
