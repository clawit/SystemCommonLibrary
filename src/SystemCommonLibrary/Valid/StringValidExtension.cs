using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Valid
{
    public static class StringValidExtension
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool NotEmpty(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
