using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using SystemCommonLibrary.Network;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class TokenAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTokenAuthentication(this IServiceCollection services,  
            Func<int, string, HttpClientType, bool> checkToken, Func<string, string, bool> checkPrvlg,
            string prefix = "Token", string apiPrefix = "ApiToken", AuthType authType = AuthType.Internal)
        {
            AuthConst.AuthPrefix = prefix;
            AuthConst.ApiAuthPrefix = apiPrefix;
            AuthConst.AuthType = authType;

            return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthConst.AuthPrefix;
                options.DefaultChallengeScheme = AuthConst.AuthPrefix;
            })
            .AddTokenAuthenticationHandler(AuthConst.AuthPrefix, o => {
                o.CheckAuth = checkToken;
                o.CheckPrvlg = checkPrvlg;
            });
        }

        private static AuthenticationBuilder AddTokenAuthenticationHandler(this AuthenticationBuilder builder, string authenticationScheme, Action<TokenAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(authenticationScheme, configureOptions);
        }

        public static void AddMvcTokenAuthentication(this IServiceCollection services)
        {
            services.AddScoped<AuthorizeActionFilter>();
        }

        public static void UseMvcTokenAuthentication(this IApplicationBuilder builder,
            Func<int, string, HttpClientType, Task<bool>> checkToken,
            string prefix = "Token", AuthType authType = AuthType.Internal)
        {
            AuthConst.AuthPrefix = prefix;
            AuthConst.AuthType = authType;
            AuthConst.CheckAuth = checkToken;
        }
    }
}
