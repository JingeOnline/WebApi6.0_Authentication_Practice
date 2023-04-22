using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_RefreshToken.Authorization;
using WebApi_RefreshToken.Models;
using WebApi_RefreshToken.Services;

namespace WebApi_RefreshToken.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        //用户发送Username和Password，
        public IActionResult Authenticate(AuthenticateRequest requestModel)
        {
            AuthenticateResponse response = _userService.Authenticate(requestModel, getIpAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        //获取HTTP请求的IP地址
        private string getIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        //设置Cookie，设置之后，响应中就会携带该Cookie
        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
