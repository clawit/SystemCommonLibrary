using System;

namespace SystemCommonLibrary.Reflect
{
    public static class TypeExtension
    {
        public static bool IsObject(this Type type)
        {
            return type != null && type == typeof(object);
        }

        public static bool IsRuntimeType(this Type type)
        {
            return type.FullName == "System.RuntimeType";
        }
    }
}
