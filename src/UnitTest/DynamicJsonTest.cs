using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Json;
using Xunit;

namespace UnitTest
{
    public class DynamicJsonTest
    {
        [Fact]
        public void Des_Test()
        {
            string json = File.ReadAllText("TestData/json.txt");
            var schema = DynamicJson.Parse(json).Deserialize<EntitySchema>();

            Assert.NotNull(schema);
        }
    }
}
