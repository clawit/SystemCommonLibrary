using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public static class FirewallExtensions
    {
        public static IApplicationBuilder UseFirewall(this IApplicationBuilder builder, IFirewallRule rule)
        {
            return builder.UseMiddleware<FirewallMiddleware>(rule);
        }
    }
}
