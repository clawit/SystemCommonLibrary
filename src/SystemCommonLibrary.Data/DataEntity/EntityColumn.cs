using System;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntityColumn
    {
        public string Column { get; set; }
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public string Formatter { get; set; }
        public bool Editable { get; set; }
        public EditorType Editor { get; set; }
    }
}
