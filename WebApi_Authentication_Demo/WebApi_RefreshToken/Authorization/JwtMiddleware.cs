using Microsoft.Extensions.Options;
using WebApi_RefreshToken.Helpers;
using WebApi_RefreshToken.Services;

namespace WebApi_RefreshToken.Authorization
{
    /// <summary>
    /// 该中间件负责验证所有请求中的JWT_Token
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                // 如果验证成功，把User添加到HttpContext中，传导到下一个中间件。下一个中间件就是AuthorizeAttribute.cs
                context.Items["User"] = userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
