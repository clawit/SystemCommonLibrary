using Microsoft.AspNetCore.Authentication;
using System;
using SystemCommonLibrary.Enums;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public TokenAuthenticationOptions()
        {
        }

        public Func<TokenAuthIdentity, HttpClientType, bool> CheckAuth { get; set; }

        public Func<PrivilegeIdentity, bool> CheckPrvlg { get; set; }
    }
}
