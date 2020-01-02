using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Firewall
{
    public abstract class FirewallRule
    {
        public abstract bool IsAllowed(HttpContext context);
    }
}
