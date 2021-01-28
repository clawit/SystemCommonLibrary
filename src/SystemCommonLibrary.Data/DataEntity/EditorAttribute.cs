using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorAttribute : Attribute
    {
        public bool Editable { get; set; } = true;
        public EditorType EditorType { get; }
        public bool Required { get; set; } = false;
        public int Length { get; set; } = 0;

        public EditorAttribute(EditorType type)
        {
            this.EditorType = type;
        }
    }
}
