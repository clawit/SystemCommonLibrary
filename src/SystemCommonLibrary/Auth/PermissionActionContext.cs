using System.Reflection;

namespace SystemCommonLibrary.Auth
{
    public class PermissionActionContext
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public TypeInfo ControllerTypeInfo { get; set; }
        public string DisplayName { get; set; }
        public MethodInfo MethodInfo { get; set; }

    }
}
