using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_BasicAuthentication.Authentication;

namespace WebApi_BasicAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            //该代码是用来测试在Response中添加cookie -------------------------------------
            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true, //当设置为true的时候，客户端的javascript无法访问。但是使用Postman还是可以查看和修改Cookie的。
                Expires = DateTime.Now.AddMinutes(1)
            };
            Response.Cookies.Append("Test", $"This is a test cooky, will expired at {cookieOptions.Expires}.", cookieOptions);
            //-------------------------------------------------------------------------------

            List<string> userNames = _userRepository.GetUserNames();
            return Ok(userNames);
        }

        //这个Action是用来测试读取客户端的cookie.
        //测试结果，如果cookie没有过期，是可以读取到的。如果过期了，cookie在浏览器中就被删除了，就无法被读取到了。string test=null。
        [HttpGet]
        [Route("cookie")]
        public IActionResult GetCookie()
        {
            string test = Request.Cookies["Test"];
            return Ok(test);
        }
    }
}
