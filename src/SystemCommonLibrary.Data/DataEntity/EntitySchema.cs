using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntitySchema
    {
        public List<EntityColumn> Columns { get; set; } = new List<EntityColumn>();

        public EntityColumn GetEntityKey()
        {
            if (Columns.Count > 0)
            {
                var key = Columns.FirstOrDefault(c => c.IsEntityKey);
                if (key == null)
                {
                    key = Columns.FirstOrDefault(c => c.IsKey);
                }

                return key;
            }
            else
                return null;
        }
    }
}
