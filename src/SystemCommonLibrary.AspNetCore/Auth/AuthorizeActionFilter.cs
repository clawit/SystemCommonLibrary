using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using SystemCommonLibrary.Valid;
using SystemCommonLibrary.Enums;
using SystemCommonLibrary.IoC.Attributes;
using SystemCommonLibrary.Network;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    [Filter]
    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (AuthConst.AuthType == AuthType.Internal)
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
                    var identity = AuthReader.Read(authorization);
                    if (identity.NotNull()
                        && await AuthConst.CheckAuth(identity, HttpClientReader.Read(agents)))
                    { 
                        await next();
                    }
                    else
                        context.Result = new RedirectResult(AuthConst.LoginUrl);
                }
                else
                    context.Result = new RedirectResult(AuthConst.LoginUrl);
            }
            else
                throw new NotImplementedException();
        }
    }
}
