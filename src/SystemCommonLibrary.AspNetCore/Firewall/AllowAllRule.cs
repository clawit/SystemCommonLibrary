using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public sealed class AllowAllRule : FirewallRule
    {
        public override bool IsAllowed(HttpContext context)
        {
            return true;
        }
    }
}
