using System;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntityCell
    {
        public EntityCell() { }

        public EntityCell(EntityColumn column, object key, object value)
        {
            Column = column;
            Key = key;
            Value = value;
        }

        public EntityColumn Column { get; set; }

        public object Key { get; set; }

        public object Value { get; set; }
    }
}
