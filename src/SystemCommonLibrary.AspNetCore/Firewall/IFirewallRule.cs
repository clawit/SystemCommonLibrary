using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public interface IFirewallRule
    {
        bool IsAllowed(HttpContext context, FirewallRuleRelation relation);
    }
}
