using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Json;
using Xunit;

namespace UnitTest
{
    public class DynamicJsonTest
    {
        [Fact]
        public void Deserialize_Test()
        {
            var schema = new EntitySchema();
            schema.Columns.Add(new EntityColumn() { 
                Items = new Dictionary<string, object>() {
                    { "name1", 1},{ "name2", "2" }
                }
            });

            var str = DynamicJson.Serialize(schema);
            EntitySchema des = DynamicJson.Parse(str).Deserialize<EntitySchema>();

            Assert.Equal("name1", des.Columns[0].Items.Keys.ElementAt(0));
            Assert.Equal(1D, des.Columns[0].Items.Values.ElementAt(0));
            Assert.Equal("name2", des.Columns[0].Items.Keys.ElementAt(1));
            Assert.Equal("2", des.Columns[0].Items.Values.ElementAt(1));
        }
    }
}
