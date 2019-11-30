using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Reflect
{
    public static class TypeExtension
    {
        public static bool IsObject(this Type type)
        {
            return type != null && type == typeof(object);
        }
    }
}
