using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SystemCommonLibrary.TinyMapper
{
    public static class MapperCache
    {
        private static Dictionary<Type, TypeDescription> _cacheList = new Dictionary<Type, TypeDescription>();

        static MapperCache()
        {

        }

        public static TypeDescription Get(Type type)
        {
            if (type == null)
                return null;

            if (_cacheList.Keys.Contains(type))
                return _cacheList[type];
            else
            {
                TypeDescription cacheCodon = new TypeDescription(type);

                Monitor.Enter(_cacheList);

                if (_cacheList.Keys.Contains(type) == false)
                    _cacheList.Add(type, cacheCodon);

                Monitor.Exit(_cacheList);

                return cacheCodon;

            }
        }
    }
}
