using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntitySchema
    {
        public List<EntityColumn> Columns { get; set; } = new List<EntityColumn>();
    }
}
