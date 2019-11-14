using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public class PrivilegeIdentity
    {
        public string Name { get; set; }

        public string Prvlg { get; set; }

        public PrivilegeIdentity(string name, string prvlg)
        {
            this.Name = name;
            this.Prvlg = prvlg;
        }
    }
}
