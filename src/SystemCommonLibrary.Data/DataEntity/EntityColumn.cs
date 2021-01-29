using System;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntityColumn
    {
        public EntityColumn()
        {
            
        }
        public EntityColumn(string column)
        {
            Column = column;
        }

        public string Column { get; set; }
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public string Formatter { get; set; }
        public bool Editable { get; set; }
        public EditorType Editor { get; set; }
        public bool IsKey { get; set; } = false;
        public int Length { get; set; } = 0;
        public bool Required { get; set; } = false;
    }
}
