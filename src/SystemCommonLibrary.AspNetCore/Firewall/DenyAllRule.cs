using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public sealed class DenyAllRule : IFirewallRule
    {
        /// <summary>
        /// Denotes whether a given <see cref="HttpContext"/> is permitted to access the web server.
        /// </summary>
        public bool IsAllowed(HttpContext context, FirewallRuleRelation relation)
        {
            return false;
        }
    }
}
