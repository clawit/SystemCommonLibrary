using System;
using System.Threading.Tasks;
using SystemCommonLibrary.Network;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class AuthConst
    {
        public const string AuthKey = "authorization";
        public const string UserAgentKey = "User-Agent";

        internal static string AuthPrefix;
        internal static string ApiAuthPrefix;
        internal static AuthType AuthType;
        internal static Func<int, string, HttpClientType, Task<bool>> CheckAuth;
        internal static string LoginUrl;
    }
}
