using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SystemCommonLibrary.Network;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class TokenAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTokenAuthentication(this IServiceCollection services, string scheme, 
            Func<int, string, HttpClientType, bool> checkToken,
            Func<string, string, bool> checkPrvlg)
        {
            return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = scheme;
                options.DefaultChallengeScheme = scheme;
            })
            .AddTokenAuthenticationHandler(scheme, o => {
                o.CheckAuth = checkToken;
                o.CheckPrvlg = checkPrvlg;
            });
        }

        private static AuthenticationBuilder AddTokenAuthenticationHandler(this AuthenticationBuilder builder, string authenticationScheme, Action<TokenAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(authenticationScheme, configureOptions);
        }
    }
}
