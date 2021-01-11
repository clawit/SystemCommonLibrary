using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SystemCommonLibrary.Data.DataEntity
{
    public static class EntitySchemaReader
    {
        public static EntitySchema GetSchema<T>()
        {
            var type = typeof(T);
            var schema = new EntitySchema();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var attrColumn = (ColumnAttribute)property.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
                var attrEditor = (EditorAttribute)property.GetCustomAttributes(typeof(EditorAttribute), true).FirstOrDefault();
                if (attrColumn != null)
                {
                    var col = new EntityColumn()
                    {
                        Column = property.Name,
                        Name = attrColumn.Name,
                        Hidden = attrColumn.Hidden,
                        Formatter = attrColumn.Formatter
                    };

                    if (attrEditor != null)
                    {
                        col.Editable = attrEditor.Editable;
                        col.Editor = attrEditor.EditorType;
                    }

                    schema.Columns.Add(col);
                }
            }

            return schema;
        }
    }
}
