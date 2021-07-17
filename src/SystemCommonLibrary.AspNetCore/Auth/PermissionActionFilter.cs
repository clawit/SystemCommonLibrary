using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using SystemCommonLibrary.Auth;
using SystemCommonLibrary.IoC.Attributes;
using SystemCommonLibrary.Network;
using SystemCommonLibrary.Valid;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    [Filter]
    public class PermissionActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers.ContainsKey(AuthConst.AuthKey)
                || context.HttpContext.Request.Cookies.ContainsKey(AuthConst.AuthKey))
            {
                var agents = context.HttpContext.Request.Headers[AuthConst.UserAgentKey].ToString();
                string authorization = context.HttpContext.Request.Headers[AuthConst.AuthKey].ToString();
                if (authorization.IsEmpty())
                {
                    //如果header中没有读到尝试从cookie中读取
                    authorization = context.HttpContext.Request.Cookies[AuthConst.AuthKey];
                }
                if (!authorization.StartsWith(AuthConst.AuthPrefix))
                {
                    context.Result = new RedirectResult(AuthConst.LoginUrl);
                }
                var identity = AuthReader.Read(authorization);

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var permissionActionContext = new PermissionActionContext() {
                    ActionName = descriptor.ActionName,
                    ControllerName = descriptor.ControllerName,
                    ControllerTypeInfo = descriptor.ControllerTypeInfo,
                    DisplayName = descriptor.DisplayName,
                    MethodInfo = descriptor.MethodInfo,
                    Path = context.HttpContext.Request.Path
                };
                if (identity.NotNull()
                    && await AuthConst.CheckPermission(permissionActionContext, identity, HttpClientReader.Read(agents)))
                {
                    await next();
                }
                else
                    context.Result = new RedirectResult(AuthConst.LoginUrl);
            }
            else
                context.Result = new RedirectResult(AuthConst.LoginUrl);
        }
    }
}
