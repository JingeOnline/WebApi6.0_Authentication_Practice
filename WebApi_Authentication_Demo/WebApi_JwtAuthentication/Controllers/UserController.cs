using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_JwtAuthentication.Authentication;
using WebApi_JwtAuthentication.Models;

namespace WebApi_JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateRequestModel request)
        {
            AuthenticateResponseModel response = _userRepository.Authenticate(request);
            if (response == null)
            {
                //返回400 Bad Request
                //后面添加的信息会以JSON格式显示在Body中：{"message": "Username or password is incorrect."}
                return BadRequest(new { message = "Username or password is incorrect." });
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
