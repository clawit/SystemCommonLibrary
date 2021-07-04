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

        public Func<int, string, HttpClientType, bool> CheckAuth { get; set; }

        public Func<string, string, bool> CheckPrvlg { get; set; }
    }
}
