using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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
            string path = context.Request.Path.Value;
            return _routes.Any(r => IsMatch(r, path));
        }

        private bool IsMatch(string rule, string path)
        {
            Regex regex = new Regex(rule, RegexOptions.IgnoreCase);
            return regex.IsMatch(path);
        }
    }
}
