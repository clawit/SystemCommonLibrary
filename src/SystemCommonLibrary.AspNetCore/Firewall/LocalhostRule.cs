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
    public sealed class LocalhostRule : IFirewallRule
    {
        private readonly IFirewallRule _nextRule;

        /// <summary>
        /// Initialises a new instance of <see cref="LocalhostRule"/>.
        /// </summary>
        public LocalhostRule(IFirewallRule nextRule) =>
            _nextRule = nextRule ?? throw new ArgumentNullException(nameof(nextRule));

        /// <summary>
        /// Denotes whether a given <see cref="HttpContext"/> is permitted to access the web server.
        /// </summary>
        public bool IsAllowed(HttpContext context)
        {
            var localIpAddress = context.Connection.LocalIpAddress;
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            var isLocalhost =
                (remoteIpAddress != null
                    && remoteIpAddress.ToString() != "::1"
                    && remoteIpAddress.Equals(localIpAddress))
                || IPAddress.IsLoopback(remoteIpAddress);

            return isLocalhost || _nextRule.IsAllowed(context);
        }
    }
}
