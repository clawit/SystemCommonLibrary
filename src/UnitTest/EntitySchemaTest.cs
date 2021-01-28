using System;
using System.Collections.Generic;
using System.Linq;
using SystemCommonLibrary.Data.DataEntity;
using Xunit;

namespace UnitTest
{
    public class Brand : Entity
    {
        [Editor(EditorType.Text, Required = true)]
        public string Name { get; set; }

        public string Icon { get; set; }

        public int Index { get; set; }

        [Column("描述", Length = 32)]
        public string Desc { get; set; }
    }
    public class EntitySchemaTest
    {
        [Fact]
        public void GetSchema_Test()
        {
            var schema = EntitySchemaReader.GetSchema<Brand>();
            var colId = schema.Columns.FirstOrDefault(c => c.Column == "Id");
            var colName = schema.Columns.FirstOrDefault(c => c.Column == "Name");
            var colDesc = schema.Columns.FirstOrDefault(c => c.Column == "Desc");

            Assert.True(colId.IsKey);
            Assert.True(colName.Required);
            Assert.True(colName.Editable);
            Assert.Equal(EditorType.Text, colName.Editor);
            Assert.Equal(32, colDesc.Length);
            Assert.Equal("描述", colDesc.Name);

        }
    }
}
