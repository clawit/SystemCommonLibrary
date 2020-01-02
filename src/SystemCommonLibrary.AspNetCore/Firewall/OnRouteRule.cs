using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    /// <summary>
    /// A Firewall rule which permits access to connections from localhost.
    /// </summary>
    public sealed class OnRouteRule : FirewallRule
    {
        private List<string> _routes;

        public OnRouteRule(string[] routes)
        {
            _routes = new List<string>(routes);
        }

        public override bool IsAllowed(HttpContext context)
        {
            

            return true;
        }
    }
}
