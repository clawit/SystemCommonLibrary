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

        public static bool IsStruct(this Type type)
        {
            return type.IsValueType &&
                   !type.IsEnum &&
                   !type.IsPrimitive &&
                   type != typeof(decimal) &&
                   type != typeof(DateTime) &&
                   type != typeof(TimeSpan) &&
                   type != typeof(Guid);
        }
    }
}
