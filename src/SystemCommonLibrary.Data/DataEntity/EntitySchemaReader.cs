using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

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
                var attrKey = (KeyAttribute)property.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault();

                if (attrColumn != null || attrEditor != null || attrKey != null)
                {
                    var col = new EntityColumn(property.Name);
                    schema.Columns.Add(col);

                    if (attrColumn != null)
                    {
                        col.Name = attrColumn.Name;
                        col.Hidden = attrColumn.Hidden;
                        col.Formatter = attrColumn.Formatter;
                    }

                    if (attrEditor != null)
                    {
                        col.Editable = attrEditor.Editable;
                        col.Editor = attrEditor.EditorType;
                        col.Length = attrEditor.Length;
                        col.Required = attrEditor.Required;
                    }

                    if (attrKey != null)
                    {
                        col.IsKey = true;
                    }
                }
            }

            return schema;
        }
    }
}
