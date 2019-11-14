using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorization = this.Request.Headers["Authorization"].ToString();
            var agents = this.Request.Headers["User-Agent"].ToString();

            PrivilegeIdentity prvlgId = null;
            var prvlg = this.Request.Headers["ApiToken"].ToString();

            if (!string.IsNullOrEmpty(prvlg))
            {
                prvlgId = PrvlgReader.Read(prvlg);
            }

            if (TokenAuth.Authorize(authorization, agents, this.Options.CheckAuth))
            {
                if (prvlgId != null)
                {
                    if ((prvlgId.Name == "AndroidApi" || prvlgId.Name == "IosApi" || prvlgId.Name == "WxMP")
                        && PrvlgAuth.Authorize(prvlg, this.Options.CheckPrvlg))
                    {
                        var identity = new ClaimsIdentity(this.Scheme.Name);
                        identity.AddClaim(new Claim(this.Scheme.Name, authorization));
                        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                    else
                        return Task.FromResult(AuthenticateResult.Fail("No credentials."));
                }
                else
                {
                    var identity = new ClaimsIdentity(this.Scheme.Name);
                    identity.AddClaim(new Claim(this.Scheme.Name, authorization));
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                    //TODO:现在,因为历史遗留版本问题, 为了兼容旧版暂时对于客户端的API不验证.
                    //但如果有, 还是验证一下, 将来如果版本都迭代好了, 就全部验证.
                    //只需注释上面的代码,解注释下面的代码即可
                    //return Task.FromResult(AuthenticateResult.Fail("No credentials."));
                }
            }
            else 
            {
                if (!string.IsNullOrEmpty(prvlg) && prvlgId != null
                    && prvlgId.Name != "AndroidApi" && prvlgId.Name != "IosApi"
                    && PrvlgAuth.Authorize(prvlg, this.Options.CheckPrvlg))
                {
                    var identity = new ClaimsIdentity(this.Scheme.Name);
                    identity.AddClaim(new Claim(this.Scheme.Name, authorization));
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                    return Task.FromResult(AuthenticateResult.Fail("No credentials."));
            }
        }
    }
}
