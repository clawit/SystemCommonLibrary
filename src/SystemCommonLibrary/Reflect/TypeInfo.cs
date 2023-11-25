using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace SystemCommonLibrary.Reflect
{
    public class SlimTypeInfo
    {
        public Type Type { get; set; }

        public Object Instance { get; set; }

        public Type[] ArgTypes { get; set; }

        public MethodInfo MethodInfo { get; set; }


        private static readonly object _syncLock = new object();

        private static readonly ConcurrentDictionary<string, SlimTypeInfo> _instanceCache = new ConcurrentDictionary<string, SlimTypeInfo>();

        internal static readonly string[] ListTypes = { "List`1", "HashSet`1", "IList`1", "ISet`1", "ICollection`1", "IEnumerable`1" };

        internal static readonly string[] DicTypes = { "Dictionary`2", "IDictionary`2" };

        /// <summary>
        /// 添加或获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SlimTypeInfo GetOrAddInstance(Type type, string methodName = "Add")
        {
            if (type.IsInterface)
            {
                throw new Exception("服务方法中不能包含接口内容！");
            }
            else if (type.IsClass || type.IsStruct())
            {
                var fullName = type.FullName + methodName;

                SlimTypeInfo typeInfo = _instanceCache.GetOrAdd(fullName, (v) =>
                {
                    Type[] argsTypes = null;

                    if (type.IsGenericType)
                    {
                        argsTypes = type.GetGenericArguments();
                        type = type.GetGenericTypeDefinition().MakeGenericType(argsTypes);
                    }

                    var mi = type.GetMethod(methodName);

                    return new SlimTypeInfo()
                    {
                        Type = type,
                        MethodInfo = mi,
                        ArgTypes = argsTypes
                    };
                });
                typeInfo.Instance = Activator.CreateInstance(type);

                return typeInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加或获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static SlimTypeInfo GetOrAddInstance(Type type, MethodInfo info)
        {
            lock (_syncLock)
            {
                if (type.IsInterface)
                {
                    throw new Exception("服务方法中不能包含接口内容！");
                }

                var fullName = type.FullName + info.Name;

                SlimTypeInfo typeInfo = _instanceCache.GetOrAdd(fullName, (v) =>
                {
                    return new SlimTypeInfo()
                    {
                        Type = type,
                        MethodInfo = info
                    };
                });
                typeInfo.Instance = Activator.CreateInstance(type);
                return typeInfo;
            }
        }
    }
}

