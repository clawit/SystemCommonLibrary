using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorAttribute : Attribute
    {
        public bool Editable { get; }
        public EditorType EditorType { get; }

        public EditorAttribute(EditorType type, bool editable)
        {
            this.EditorType = type;
            this.Editable = editable;
        }
    }
}
