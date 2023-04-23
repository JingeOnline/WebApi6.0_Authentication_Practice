using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi_RefreshToken.Authorization;
using WebApi_RefreshToken.Models;
using WebApi_RefreshToken.Services;

namespace WebApi_RefreshToken.Controllers
{
    [Authorize]  //这里Authorize属性，是自己重写的AuthorizeAttribute.cs
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //用户发送Username和Password，获取JwtToken和RefreshToken
        [AllowAnonymous]  //允许用户不经过验证而访问该Action
        [HttpPost("authenticate")] //这里的template是指定该Action的URL
        public IActionResult Authenticate(AuthenticateRequest requestModel)
        {
            AuthenticateResponse response = _userService.Authenticate(requestModel, getIpAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        //读取用户Cookie中的RefreshToken，更新用户的JwtToken
        [AllowAnonymous]   //允许用户不经过验证而访问该Action
        [HttpGet("refresh-JWT-token")]//这里的template是指定该Action的URL
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            AuthenticateResponse response = _userService.RefreshToken(refreshToken, getIpAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        //注销用户的RefreshToken
        [HttpPost("revoke-token")]//这里的template是指定该Action的URL
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept refresh token in request body or cookie
            string token;
            if(model!=null && !string.IsNullOrEmpty(model.RefreshToken))
            {
                token = model.RefreshToken;
            }
            else
            {
                token= Request.Cookies["refreshToken"];
            }
            Debug.WriteLine($"RefreshToken={token}");
            _userService.RevokeToken(token, getIpAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        //获取所有User列表
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        //指定UserId，获取该用户的信息
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        //获取该用户所有的RefreshToken列表
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user.RefreshTokens);
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
