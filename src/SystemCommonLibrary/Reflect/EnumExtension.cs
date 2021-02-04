using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SystemCommonLibrary.Reflect
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum EnumValue)
        {
            Type type = EnumValue.GetType();
            MemberInfo[] memberInfo = type.GetMember(EnumValue.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attrs != null && attrs.Count() > 0))
                {
                    return ((DescriptionAttribute)attrs.ElementAt(0)).Description;
                }
            }
            return EnumValue.ToString();
        }

    }
}
