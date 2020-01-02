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
    public sealed class OnMethodRule : FirewallRule
    {
        private List<string> _methods;

        public OnMethodRule(string[] methods)
        {
            _methods = new List<string>(methods);
        }

        public override bool IsAllowed(HttpContext context)
        {
            string method = context.Request.Method;
            return _methods.Contains(method, StringComparer.OrdinalIgnoreCase);
        }
    }
}
