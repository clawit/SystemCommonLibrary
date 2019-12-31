using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using SystemCommonLibrary.Geo;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public static class FirewallRuleFactory
    {
        /// <summary>
        /// Configures the Firewall to deny all access.
        /// <para>Use this as the base rule before configuring other rules.</para>
        /// </summary>
        public static IFirewallRule DenyAll()
        {
            return new DenyAllRule();
        }

        /// <summary>
        /// Configures the Firewall to deny all access.
        /// <para>Use this as the base rule before configuring other rules.</para>
        /// </summary>
        public static IFirewallRule AllowAll()
        {
            return new AllowAllRule();
        }

        /// <summary>
        /// Configures the Firewall to allow requests from localhost.
        /// </summary>
        public static IFirewallRule ExceptFromLocalhost(this IFirewallRule rule)
        {
            return new LocalhostRule(rule);
        }

    }
}
