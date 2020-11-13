using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SystemCommonLibrary.TinyMapper
{
    public class TypeDescription
    {
        public Type Type
        {
            get;
            private set;
        }

        public List<PropertyDescription> PropertyList { get; set; } = new List<PropertyDescription>();

        private Hashtable _propertyNames = new Hashtable();

        public TypeDescription(Type type)
        {
            Type = type;

            PropertyInfo[] propertyList = Type.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                PropertyDescription propertyMappingDescription = new PropertyDescription(property);

                PropertyList.Add(propertyMappingDescription);
                _propertyNames.Add(property.Name, propertyMappingDescription);
            }
        }

        public bool ContainsProperty(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("TypeMappingDescription.ContainsProperty 必须指定属性名。");

            return _propertyNames.ContainsKey(propertyName);
        }

        public bool IsVirtual(string propertyName)
        {
            PropertyDescription propertyMappingDescription = (PropertyDescription)_propertyNames[propertyName];
            return propertyMappingDescription.PropertyInfo.GetSetMethod().IsVirtual;
        }

        public object GetValue(object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException("指定的对象为空。");

            if (obj.GetType() != this.Type)
                throw new ArgumentException("指定的对象类型与缓存的对象类型不一致。");

            if (_propertyNames.ContainsKey(propertyName) == false)
                throw new ArgumentOutOfRangeException("指定的属性名不存在。");

            var propertyMappingDescription = (PropertyDescription)_propertyNames[propertyName];
            if (propertyMappingDescription.CanRead == false)
                throw new InvalidOperationException("属性 " + propertyName + "不可读。");

            return propertyMappingDescription.GetValue(obj);
        }

        public void SetValue(object obj, string propertyName, object value)
        {
            if (obj == null)
                throw new ArgumentNullException("指定的对象为空。");

            if (obj.GetType() != this.Type)
                throw new ArgumentException("指定的对象类型与缓存的对象类型不一致。");

            if (_propertyNames.ContainsKey(propertyName) == false)
                throw new ArgumentOutOfRangeException("指定的属性名不存在。");

            PropertyDescription propertyMappingDescription = (PropertyDescription)_propertyNames[propertyName];
            if (propertyMappingDescription.CanWrite == false)
                throw new InvalidOperationException("属性 " + propertyName + "只读。");

            Type propertyType = propertyMappingDescription.PropertyInfo.PropertyType;
            if (propertyType.IsValueType == false && value != null)
            {
                Type valueType = value.GetType();
                if (propertyType != valueType && valueType.IsSubclassOf(propertyType) == false)
                {
                    throw new ArgumentException("目标对象的 " + propertyName + "与 value 的类型既不一致，也不是目标类型的派生类。");
                }
            }

            propertyMappingDescription.SetValue(obj, value);
        }
    }
}
