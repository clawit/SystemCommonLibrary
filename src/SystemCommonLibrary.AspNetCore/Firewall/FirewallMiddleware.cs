using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public sealed class FirewallMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFirewallRule _rule;

        /// <summary>
        /// Instantiates a new object of type <see cref="FirewallMiddleware"/>.
        /// </summary>
        public FirewallMiddleware(RequestDelegate next, IFirewallRule rule)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public Task Invoke(HttpContext context)
        {
            return _rule.IsAllowed(context) ? _next.Invoke(context) : _accessDeny(context);
        }

        private Task _accessDeny(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return context.Response.WriteAsync("Forbidden");
        }
    }
}
