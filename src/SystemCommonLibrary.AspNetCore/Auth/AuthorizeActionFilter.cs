using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using SystemCommonLibrary.Encrypt;
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
                if (context.HttpContext.Request.Cookies.ContainsKey(AuthConst.AuthKey))
                {
                    var agents = context.HttpContext.Request.Headers[AuthConst.UserAgentKey].ToString();
                    string authorization = context.HttpContext.Request.Cookies[AuthConst.AuthKey];
                    var authorizations = Base64Encrypt.FromBase64(authorization).Split(":");
                    if (authorizations.Length == 2
                        && int.TryParse(authorizations[0], out int id)
                        && await AuthConst.CheckAuth(id, authorizations[1], HttpClientReader.Read(agents)))
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
