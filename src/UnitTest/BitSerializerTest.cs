using System;
using System.Collections.Generic;
using SystemCommonLibrary.Serialization;
using Xunit;

namespace UnitTest
{
    public class LevelOne 
    {
        public string Dim1 { get; set; } = "Val";

        private int Dim2 { get; set; } = 1000;

        protected bool Dim3 { get; set; } = true;

        public DateTime Dim4 = DateTime.Now;

        public LevelTwo Dim5 = new LevelTwo();
    }

    public class LevelTwo
    {
        public List<string> Dim1 { get; set; } = new List<string>() { "Val1", "Val2" };

        public int[] Dim2 { get; set; } = { 1, 10, 100, 1000 };

        public Dictionary<string, object> Dim3 { get; set; } = new Dictionary<string, object>();

        public LevelThree Dim4 { get; set; } = new LevelThree();
    }

    public class LevelThree
    {
        public Type Dim1 { get; set; } = typeof(int);

        public int Dim2 = 10000;

        public Guid Dim3 { get; set; } = Guid.NewGuid();

        public byte[] Dim4 = Convert.FromBase64String("VmFs");

        public byte Dim5 = 20;

        public LevelEnum Dim6 = LevelEnum.Enum2;
    }

    public enum LevelEnum
    { 
        Enum1 = 10,
        Enum2 = 100
    }

    public class BitSerializerTest
    {
        [Fact]
        public void SerializeTest()
        {
            var src = new LevelOne() { Dim4 = Convert.ToDateTime("2000-01-01 01:02:03.789") };
            var bytes = BitSerializer.Serialize(src);

            var obj = BitSerializer.Deserialize("UnitTest.LevelOne, UnitTest", bytes);
            var dest = (LevelOne)obj;

            Assert.Equal(src.Dim1, dest.Dim1);
        }
    }
}
