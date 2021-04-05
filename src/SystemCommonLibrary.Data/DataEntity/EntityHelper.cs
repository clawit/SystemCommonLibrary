using System.Collections.Generic;
using SystemCommonLibrary.Json;

namespace SystemCommonLibrary.Data.DataEntity
{
    public static class EntityHelper
    {
        public static Dictionary<string, object> ParseRow(dynamic row)
        {
            var result = new Dictionary<string, object>();

            foreach (dynamic item in row)
            {
                result.Add(item.Key, item.Value);
            }

            return result;
        }

        public static EntityTable Convert(string json)
        {
            dynamic obj = DynamicJson.Parse(json);
            try
            {
                //如果包含schema信息,则直接能转换成功
                return obj.Deserialize<EntityTable>();
            }
            catch { }

            return null;
        }

        public static EntityData ConvertData(dynamic rows, EntitySchema schema)
        {
            var result = new EntityData();

            if (rows.IsArray)
            {
                foreach (var row in rows)
                {
                    Dictionary<string, object> dic = ParseRow(row);

                    var list = new List<object>();
                    foreach (var column in schema.Columns)
                    {
                        if (dic.ContainsKey(column.Column)
                            && dic[column.Column] != null)
                        {
                            list.Add(dic[column.Column]);
                        }
                        else
                        {
                            list.Add(string.Empty);
                        }

                    }

                    result.Rows.Add(list);
                }
            }

            return result;
        }
    }
}
