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
    public sealed class AllowLocalhostRule : FirewallRule
    {
        public override bool IsAllowed(HttpContext context)
        {
            var localIpAddress = context.Connection.LocalIpAddress;
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            var isLocalhost =
                (remoteIpAddress != null
                    && remoteIpAddress.ToString() != "::1"
                    && remoteIpAddress.Equals(localIpAddress))
                || IPAddress.IsLoopback(remoteIpAddress);

            return isLocalhost;
        }
    }
}
