using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using SystemCommonLibrary.Network;

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
