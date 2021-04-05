using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Valid
{
    public static class NullValidExtension
    {
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static bool NotNull(this object value)
        {
            return value != null;
        }

    }
}
