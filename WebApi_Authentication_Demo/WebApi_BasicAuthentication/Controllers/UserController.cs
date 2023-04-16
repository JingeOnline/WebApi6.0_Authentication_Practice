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
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(1)
            };
            Response.Cookies.Append("Test", $"This is a test cooky, will expired at {cookieOptions.Expires}.", cookieOptions);
            //-------------------------------------------------------------------------------

            List<string> userNames = _userRepository.GetUserNames();
            return Ok(userNames);
        }

        //这个Action是用来测试读取客户端的cookie
        [HttpGet]
        [Route("cookie")]
        public IActionResult GetCookie()
        {
            string test = Request.Cookies["Test"];
            return Ok(test);
        }
    }
}
