using System;
using System.Collections.Generic;
using System.Linq;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Data.DataEntity.EditorConfig;
using Xunit;

namespace UnitTest
{
    public enum EnumStatus
    {
        [System.ComponentModel.Description("TestName")]
        Test1 = 0,
        Test2 = 2,
        Test3 = 4
    }
    public class Brand : Entity
    {
        [Editor(EditorType.Text, Required = true, Length = 32)]
        public string Name { get; set; }

        public string Icon { get; set; }

        public int Index { get; set; }

        [Column("描述")]
        public string Desc { get; set; }

        [Column("状态")]
        [Editor(EditorType.List)]
        public EnumStatus Status { get; set; }
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
            var colStatus = schema.Columns.FirstOrDefault(c => c.Column == "Status");

            Assert.True(colId.IsKey);
            Assert.True(colName.Required);
            Assert.True(colName.Editable);
            Assert.Equal(32, colName.Length);
            Assert.Equal(EditorType.Text, colName.Editor);
            Assert.Equal("描述", colDesc.Name);
            Assert.Equal(EditorType.List, colStatus.Editor);
            Assert.NotNull(colStatus.EditorConfig);
            EditorListConfig config = (EditorListConfig)colStatus.EditorConfig;
            Assert.Equal("TestName", config.Items[0].Name);
            Assert.Equal(0, config.Items[0].Value);
            Assert.Equal("Test3", config.Items[2].Name);
            Assert.Equal(4, config.Items[2].Value);
        }
    }
}
