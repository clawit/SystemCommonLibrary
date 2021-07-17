using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public class ActionContext
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public TypeInfo ControllerTypeInfo { get; set; }
        public string DisplayName { get; set; }
        public MethodInfo MethodInfo { get; set; }

    }
}
