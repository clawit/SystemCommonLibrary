using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SystemCommonLibrary.TinyMapper
{
    public class PropertyDescription
    {
        public string Name => string.IsNullOrEmpty(this.PropertyInfo.Name) ? string.Empty : this.PropertyInfo.Name;

        public bool CanRead => this.PropertyInfo.CanRead;

        public bool CanWrite => this.PropertyInfo.CanWrite;

        public PropertyInfo PropertyInfo { get; set; }

        public PropertyDescription(PropertyInfo property)
        {
            PropertyInfo = property;
        }

        public object GetValue(object obj)
        {
            return this.PropertyInfo.GetValue(obj, null);
        }

        public void SetValue(object obj, object value)
        {
            this.PropertyInfo.SetValue(obj, value, null);
        }
    }
}
