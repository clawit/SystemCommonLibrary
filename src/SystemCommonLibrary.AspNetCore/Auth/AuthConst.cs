using System;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class AuthConst
    {
        public const string AuthKey = "authorization";
        public const string UserAgentKey = "User-Agent";

        internal static string AuthPrefix = "Token";
        internal static string ApiAuthPrefix = "ApiToken";
        internal static AuthType AuthType = AuthType.Internal;
        internal static TokenAuthenticationOptions TokenAuthenticationOptions = new TokenAuthenticationOptions();
    }
}
