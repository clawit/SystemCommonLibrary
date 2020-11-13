using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemCommonLibrary.TinyMapper
{
    /// <summary>
    /// ref:https://github.com/iccb1013/Sheng.Mapper
    /// 拷贝行为只针对 sourceObject 和 targetObject 所共有的属性
    /// 在 sourceObject 和 targetObject 中的待拷贝的属性值的类型处理：
    ///     如果是值类型，直接拷贝，如果是引用类型，sourceObject 中的属性的类型 必须 和 targetObject 中的属性的类型一致，或是它的派生类
    /// 如果要支持类型不一致的属性自动进行类型转换，你可以在 PropertyMappingDescription 这个类中实现转换器功能
    /// 拷贝行为 不会 改变 targetObject 中不需要被拷贝的属性的值
    /// 你可以组合使用几个方法来从多个对象中拷贝指定的属性值到一个 targetObject
    /// </summary>
    public static class Mapper
    {
        public static T Map<T>(object source) where T : new()
        {
            var target = Activator.CreateInstance(typeof(T));
            SetValues(source, target, null, null, false);

            return (T)target;
        }

        public static T Map<T>(object source, bool skipVirtual = false) where T : new()
        {
            var target = Activator.CreateInstance(typeof(T));
            SetValues(source, target, null, null, skipVirtual);

            return (T)target;
        }

        public static T MapOnly<T>(object source, string[] properties) where T : new()
        {
            var target = Activator.CreateInstance(typeof(T));
            SetValues(source, target, properties, null, false);

            return (T)target;
        }

        public static T MapWithExcludes<T>(object source, string[] excludes) where T : new()
        {
            var target = Activator.CreateInstance(typeof(T));
            SetValues(source, target, null, excludes, false);

            return (T)target;
        }

        public static T MapWithExcludes<T>(object source, string[] excludes, bool skipVirtual = false) where T : new()
        {
            var target = Activator.CreateInstance(typeof(T));
            SetValues(source, target, null, excludes, skipVirtual);

            return (T)target;
        }

        /// <summary>
        /// skipVirtual 是针对目标对象的
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="targetObject"></param>
        /// <param name="withProperties"></param>
        /// <param name="withoutProperties"></param>
        /// <param name="skipVirtual"></param>
        private static void SetValues(object sourceObject, object targetObject, string[] withProperties, string[] withoutProperties, bool skipVirtual)
        {

            if (sourceObject == null || targetObject == null)
                throw new ArgumentNullException();

            Type sourceObjectType = sourceObject.GetType();
            Type targetObjectType = targetObject.GetType();

            TypeDescription sourceObjectTypeCache = MapperCache.Get(sourceObjectType);
            TypeDescription targetObjectCache = MapperCache.Get(targetObjectType);

            foreach (PropertyDescription sourceProperty in sourceObjectTypeCache.PropertyList)
            {
                if (withProperties != null && withProperties.Length > 0 && withProperties.Contains(sourceProperty.Name) == false)
                    continue;

                if (withoutProperties != null && withoutProperties.Length > 0 && withoutProperties.Contains(sourceProperty.Name))
                    continue;

                if (sourceProperty.CanRead == false)
                    continue;

                if (targetObjectCache.ContainsProperty(sourceProperty.Name) == false)
                    continue;

                if (skipVirtual && targetObjectCache.IsVirtual(sourceProperty.Name))
                    continue;

                object sourcePropertyValue = sourceObjectTypeCache.GetValue(sourceObject, sourceProperty.Name);
                targetObjectCache.SetValue(targetObject, sourceProperty.Name, sourcePropertyValue);
            }
        }
    }
}
