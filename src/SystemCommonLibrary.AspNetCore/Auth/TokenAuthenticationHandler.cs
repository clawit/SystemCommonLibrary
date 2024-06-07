using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public class TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<TokenAuthenticationOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorization = this.Request.Headers[AuthConst.AuthKey].ToString();
            var agents = this.Request.Headers[AuthConst.UserAgentKey].ToString();
            var prvlg = this.Request.Headers[AuthConst.ApiAuthKey].ToString();

            if (PrvlgAuth.Authorize(prvlg, this.Options.CheckPrvlg)
                && TokenAuth.Authorize(authorization, agents, this.Options.CheckAuth))
            {
                var identity = new ClaimsIdentity(this.Scheme.Name);
                identity.AddClaim(new Claim(this.Scheme.Name, authorization));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("No credentials."));
            }
        }
    }
}
