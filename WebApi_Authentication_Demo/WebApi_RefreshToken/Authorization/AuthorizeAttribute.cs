using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi_RefreshToken.Models;

namespace WebApi_RefreshToken.Authorization
{
    /// <summary>
    /// 自己实现的Authorize属性，替代默认的写在Action上的[Authorize]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute: Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            // 如果Controller中的Action允许匿名，则忽略。
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }
            //如果不允许匿名，并且JWT Token验证成功，则什么都不干，执行Action。如果失败，则直接返回401错误。
            else
            {
                // authorization
                var user = (User)context.HttpContext.Items["User"];
                if (user == null)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                    };
                }
            }
        }


    }
}
