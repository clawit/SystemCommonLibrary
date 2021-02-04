using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SystemCommonLibrary.Data.DataEntity.EditorConfig;
using SystemCommonLibrary.Reflect;

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
                        col.EditorConfig = attrEditor.EditorConfig ?? GetEditorConfig(property, attrEditor.EditorType);
                    }

                    if (attrKey != null)
                    {
                        col.IsKey = true;
                    }
                }
            }

            return schema;
        }

        private static IEditorConfig GetEditorConfig(PropertyInfo property, EditorType type)
        {
            IEditorConfig config = null;
            switch (type)
            {
                case EditorType.None:
                    break;
                case EditorType.Text:
                    break;
                case EditorType.Number:
                    config = new EditorNumberConfig();
                    break;
                case EditorType.Switch:
                    break;
                case EditorType.Date:
                    config = new EditorDateConfig();
                    break;
                case EditorType.Time:
                    break;
                case EditorType.DateTime:
                    config = new EditorDateTimeConfig();
                    break;
                case EditorType.List:
                    config = new EditorListConfig() { 
                        Items = GetEnumItems(property)
                    }; 
                    break;
                case EditorType.Checkbox:
                    config = new EditorCheckboxConfig()
                    {
                        Items = GetEnumItems(property)
                    };
                    break;
                case EditorType.Image:
                    break;
                case EditorType.Flyer:
                    break;
                case EditorType.Json:
                    break;
                case EditorType.Icon:
                    break;
                default:
                    break;
            }

            return config;
        }

        private static List<EditorConfigItem> GetEnumItems(PropertyInfo property)
        {
            if (property.PropertyType.IsEnum)
            {
                var items = new List<EditorConfigItem>();
                var values = property.PropertyType.GetEnumValues();
                foreach (var value in values)
                {
                    var name = ((Enum)value).GetDescription();
                    var item = new EditorConfigItem() { 
                        Name = name,
                        Value = ((int)value)
                    };

                    items.Add(item);
                }

                return items;
            }
            else
                return null;
        }
    }
}
