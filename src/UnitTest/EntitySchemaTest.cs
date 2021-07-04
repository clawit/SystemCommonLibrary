using System.Linq;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Enums;
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

        [Column("状态")]
        [Editor(EditorType.Checkbox, "{{name1:0},{name2:2}}")]
        public EnumStatus Check { get; set; }
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
            var colCheck = schema.Columns.FirstOrDefault(c => c.Column == "Check");

            Assert.True(colId.IsKey);

            Assert.True(colName.Required);
            Assert.True(colName.Editable);
            Assert.Equal(32, colName.Length);
            Assert.Equal(EditorType.Text, colName.Editor);

            Assert.Equal("描述", colDesc.Name);

            Assert.Equal(EditorType.List, colStatus.Editor);
            Assert.NotNull(colStatus.Items);
            Assert.Equal("TestName", colStatus.Items.Keys.FirstOrDefault());
            Assert.Equal(0, colStatus.Items.Values.FirstOrDefault());
            Assert.Equal("Test3", colStatus.Items.ElementAt(2).Key);
            Assert.Equal(4, colStatus.Items.ElementAt(2).Value);

            Assert.Equal(EditorType.Checkbox, colCheck.Editor);
            Assert.NotNull(colCheck.Items);
            Assert.Equal("name1", colCheck.Items.Keys.FirstOrDefault());
            Assert.Equal(0M, colCheck.Items.Values.FirstOrDefault());
            Assert.Equal("name2", colCheck.Items.ElementAt(1).Key);
            Assert.Equal(2M, colCheck.Items.ElementAt(1).Value);
        }
    }
}
