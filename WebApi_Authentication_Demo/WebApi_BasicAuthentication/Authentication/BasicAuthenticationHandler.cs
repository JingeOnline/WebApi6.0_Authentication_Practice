using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace WebApi_BasicAuthentication.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;

        public BasicAuthenticationHandler(IUserRepository userRepository,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        //客户端发送的请求头中会有如下字段：Authorization: Basic anpoYW5nOjEyMzQ1Ng==
        //Authorization是字段名称
        //Basic是验证类型的名称
        //anpoYW5nOjEyMzQ1Ng== 是被base64编码的用户名和密码，中间会用英文冒号分割。
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //取出请求头中的Authorization字段内容
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            //如果不为空，且以basic开头
            if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                //获取base64加密的Token，就是用户名和密码。
                string token = authorizationHeader.Substring("Basic ".Length).Trim();
                //解码，获取明文
                string credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                //用户名和密码之间默认使用冒号进行分割
                string[] credentials = credentialsAsEncodedString.Split(':');
                //去数据库User表中验证用户名和密码是否正确
                if (_userRepository.Authenticate(credentials[0], credentials[1]))
                {
                    //创建Claim，相当于证件上的词条
                    Claim[] claims = new[] { 
                        new Claim(ClaimTypes.Name, credentials[0]), 
                        new Claim(ClaimTypes.Role, "Admin") //这个Role可以去掉，因为在当前项目中没用到。
                    };
                    //创建CalimsIdentity，相当于创建一个证件
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                    //创建ClaimsPrincipal，相当于创建一个用户，也就是该证件的持有者
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    //创建一个Ticket，相当于给被创建的用户发放了一张入场券。
                    //Scheme.Name在这里的值为“BasicAuthentication”，该值在Program.cs类中注册Authentication服务的时候，手动指定。
                    AuthenticationTicket ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    //FailureMessage并不会显示，只会返回401 Unauthorized 
                    return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
                }
            }
            else
            {
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"joydipkanjilal.com\"");
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
