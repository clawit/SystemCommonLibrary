using System;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntityCell
    {
        public EntityColumn Column { get; set; }

        public object Key { get; set; }

        public object Value { get; set; }
    }
}
