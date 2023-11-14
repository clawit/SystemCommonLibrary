using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SystemCommonLibrary.Serialization;
using Xunit;

namespace UnitTest
{
    public class LevelOne
    {
        public string Dim1 { get; set; }

        private int Dim2 { get; set; } = 1000;

        protected bool Dim3 { get; set; } = true;

        public DateTime Dim4 = DateTime.Now;

        public LevelTwo Dim5;

        private static decimal Dim6 = 9.9M;

        public static long Dim7 = 111;

        private static float Dim8 { get; set; } = 2.22F;

        public static double Dim9 { get; set; } = 3.33;

        public object Dim10 = new List<string>() { "Val" };

        public string Dim11 { get; set; }

        public List<LevelThree> Dim12 { get; set; }
    }

    public class LevelTwo
    {
        public List<string> Dim1 { get; set; }

        public int[] Dim2 { get; set; }

        public Dictionary<string, object> Dim3 { get; set; }

        public LevelThree Dim4 { get; set; }
    }

    public class LevelThree
    {
        public Type Dim1 { get; set; }

        [NoSerialize]
        public int Dim2 = 10000;

        public Guid Dim3 { get; set; }

        public byte[] Dim4;

        public byte Dim5 = 20;

        public LevelEnum Dim6;

        [NoSerialize]
        public double Dim7 { get; set; }

        public object Dim8 { get; set; }

        public string Dim9 { get; set; }

        public FileInfo Dim10 { get; set; }
    }

    public enum LevelEnum
    {
        Enum1 = 10,
        Enum2 = 100
    }

    public class BitSerializerTest
    {
        [Fact]
        public void SerializeAndDeserializeTest() 
        {
            var src6 = new LevelThree()
            {
                Dim1 = typeof(LevelTwo),
                Dim3 = Guid.NewGuid(),
                Dim4 = Convert.FromBase64String("VmFs"),
                Dim5 = 20,
                Dim6 = LevelEnum.Enum2,
                Dim8 = "Val0",
                Dim9 = null,
                Dim10 = null
            };
            var bytes6 = BitSerializer.Serialize(src6);
            var dest6 = BitSerializer.Deserialize<LevelThree>(bytes6);
            Assert.Equal(src6.Dim1, dest6.Dim1);
            Assert.Equal(src6.Dim8, dest6.Dim8);
            Assert.Equal(src6.Dim3, dest6.Dim3);
            Assert.Equal(src6.Dim6, dest6.Dim6);
            Assert.Equal(src6.Dim5, dest6.Dim5);
            Assert.Equal(src6.Dim9, dest6.Dim9);
            Assert.Equal(src6.Dim10, dest6.Dim10);

            var src5 = new int[] { 1, 2, 3 };
            var bytes5 = BitSerializer.Serialize(src5);
            var dest5 = BitSerializer.Deserialize<int[]>(bytes5);
            Assert.Equal(src5, dest5);

            var src4 = new List<object>() { "Val1", "Val2", 123, true };
            var bytes4 = BitSerializer.Serialize(src4);
            var dest4 = BitSerializer.Deserialize<List<object>>(bytes4);
            Assert.Equal(src4, dest4);

            var src3 = new List<string>() { "Val1", "Val2" };
            var bytes3 = BitSerializer.Serialize(src3);
            var dest3 = BitSerializer.Deserialize<List<string>>(bytes3);
            Assert.Equal(src3, dest3);

            var src2 = new Dictionary<string, object>
            {
                { "Key1", "Val1" },
                { "Key2", "Val2" }
            };
            var bytes2 = BitSerializer.Serialize(src2);
            var dest2 = BitSerializer.Deserialize<Dictionary<string, object>>(bytes2);
            Assert.Equal(src2, dest2);

            var src1 = new Dictionary<string, string>
            {
                { "Key1", "Val1" },
                { "Key2", "Val2" }
            };
            var bytes1 = BitSerializer.Serialize(src1);
            var dest1 = BitSerializer.Deserialize<Dictionary<string, string>>(bytes1);
            Assert.Equal(src1, dest1);
        }

        [Fact]
        public void SerializeTest()
        {
            var src = new LevelOne()
            {
                Dim1 = "Val",
                Dim4 = Convert.ToDateTime("2000-01-01 01:02:03.789"),
                Dim5 = new LevelTwo()
                {
                    Dim1 = new List<string>() { "Val1", "Val2" },
                    Dim2 = new int[] { 1, 10, 100, 1000 },
                    Dim3 = new Dictionary<string, object>(),
                    Dim4 = new LevelThree()
                    {
                        Dim1 = typeof(LevelTwo),
                        Dim2 = 2000,
                        Dim3 = Guid.NewGuid(),
                        Dim4 = Convert.FromBase64String("VmFs"),
                        Dim5 = 20,
                        Dim6 = LevelEnum.Enum2
                    }
                },
                Dim11 = "Val2",
                Dim12 = new List<LevelThree>() { new LevelThree()
                {
                    Dim5 = 50
                } }
            };

            var bytes = BitSerializer.Serialize(src);
            var dest = BitSerializer.Deserialize<LevelOne>(bytes);

            Assert.Equal(src.Dim1, dest.Dim1);
            Assert.Equal(src.Dim11, dest.Dim11);
            Assert.Equal(src.Dim12[0].Dim5, dest.Dim12[0].Dim5);
            Assert.Equal(src.Dim4, dest.Dim4);
            Assert.Equal(111, LevelOne.Dim7);
            Assert.Equal(src.Dim10, dest.Dim10);
            Assert.Equal(src.Dim5.Dim1, dest.Dim5.Dim1);
            Assert.Equal(src.Dim5.Dim2, dest.Dim5.Dim2);
            Assert.Equal(src.Dim5.Dim3, dest.Dim5.Dim3);
            Assert.Equal(src.Dim5.Dim4.Dim1, dest.Dim5.Dim4.Dim1);
            Assert.Equal(10000, dest.Dim5.Dim4.Dim2);
            Assert.Equal(src.Dim5.Dim4.Dim3, dest.Dim5.Dim4.Dim3);
            Assert.Equal(src.Dim5.Dim4.Dim4, dest.Dim5.Dim4.Dim4);
            Assert.Equal(src.Dim5.Dim4.Dim5, dest.Dim5.Dim4.Dim5);
            Assert.Equal(src.Dim5.Dim4.Dim6, dest.Dim5.Dim4.Dim6);
        }
    }
}
