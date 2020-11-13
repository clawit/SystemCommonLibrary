using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.TinyMapper
{
    public static class ObjectMapperExtensions
    {
        public static T MapTo<T>(this object source) where T : new()
        {
            return Mapper.Map<T>(source);
        }
    }
}
