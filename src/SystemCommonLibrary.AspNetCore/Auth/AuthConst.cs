using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Threading.Tasks;
using SystemCommonLibrary.Auth;
using SystemCommonLibrary.Enums;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class AuthConst
    {
        //auth
        public const string AuthKey = "authorization";
        public const string UserAgentKey = "User-Agent";
        internal static string ApiAuthKey;  //ApiToken
        internal static string AuthPrefix;  //Token
        internal static AuthType AuthType;
        internal static Func<TokenAuthIdentity, HttpClientType, Task<bool>> CheckAuth;
        internal static string LoginUrl;

        //permission
        internal static Func<ActionContext, TokenAuthIdentity, HttpClientType, Task<bool>> CheckPermission;
    }
}
