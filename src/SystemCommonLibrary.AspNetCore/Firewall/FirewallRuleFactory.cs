using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using SystemCommonLibrary.Geo;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public static class FirewallRuleFactory
    {
        public static List<FirewallRule> AllowAll()
        {
            return new List<FirewallRule>() { new AllowAllRule() };
        }

        public static List<FirewallRule> OnlyLocalhost()
        {
            return new List<FirewallRule>() { new AllowLocalhostRule() };
        }

        public static List<FirewallRule> OnRoute(this List<FirewallRule> rules, string[] routes)
        {
            rules.Add(new OnRouteRule(routes));
            return rules;
        }
    }
}
