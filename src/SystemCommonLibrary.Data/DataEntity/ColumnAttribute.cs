using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; }
        public bool Hidden { get; set; } = false;
        public string Formatter { get; set; } = null;

        public ColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
