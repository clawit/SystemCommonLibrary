using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using SystemCommonLibrary.Graphic;
using Xunit;

namespace UnitTest
{
    public class CommonImageTest
    {
        [Fact]
        public void RoundTripWithAlphaTest()
        {
            var rnd = new Random(1);
            var image = new CommonImage(37, 23);
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    image.SetPixel(x, y, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256), rnd.Next(256)));

            var png = image.ToPng();
            Assert.Equal(6, png[25]); //颜色类型 RGBA

            AssertSamePixels(image, CommonImage.FromPng(png));
        }

        [Fact]
        public void OpaqueImageSavedAsRgbTest()
        {
            var image = new CommonImage(10, 10, Color.FromArgb(255, 10, 20, 30));
            image.SetPixel(3, 4, Color.Red);

            var png = image.ToPng();
            Assert.Equal(2, png[25]); //颜色类型 RGB

            var decoded = CommonImage.FromPng(new MemoryStream(png));
            AssertSamePixels(image, decoded);
            Assert.Equal(Color.Red.ToArgb(), decoded.GetPixel(3, 4).ToArgb());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        public void DecodeGrayscaleTest(int bitDepth)
        {
            int max = (1 << bitDepth) - 1;
            Func<int, int, int[]> sample = (x, y) => new[] { (x * 3 + y) % (max + 1) };
            var png = TestPng.Build(11, 5, 0, bitDepth, false, sample);

            var image = CommonImage.FromPng(png);
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 11; x++)
                {
                    int v = sample(x, y)[0];
                    int expected = bitDepth == 16 ? v >> 8 : v * 255 / max;
                    Assert.Equal(Color.FromArgb(255, expected, expected, expected).ToArgb(), image.GetPixel(x, y).ToArgb());
                }
        }

        [Fact]
        public void DecodeGrayscaleTransparentTest()
        {
            var png = TestPng.Build(4, 1, 0, 8, false, (x, y) => new[] { x * 10 }, trns: new byte[] { 0, 20 });

            var image = CommonImage.FromPng(png);
            Assert.Equal(255, image.GetPixel(1, 0).A);
            Assert.Equal(0, image.GetPixel(2, 0).A);
        }

        [Fact]
        public void DecodeGrayAlpha16Test()
        {
            var png = TestPng.Build(3, 2, 4, 16, false, (x, y) => new[] { 0x1234 * (x + 1), 0xFFFF - 0x2000 * y });

            var image = CommonImage.FromPng(png);
            var c = image.GetPixel(2, 1);
            Assert.Equal(0x36, c.R);
            Assert.Equal(0x36, c.B);
            Assert.Equal(0xDF, c.A);
        }

        [Fact]
        public void DecodeRgb16WithTransparentColorTest()
        {
            var png = TestPng.Build(2, 1, 2, 16, false,
                (x, y) => x == 0 ? new[] { 0xAAAA, 0xBBBB, 0xCCCC } : new[] { 0x0102, 0x0304, 0x0506 },
                trns: new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            var image = CommonImage.FromPng(png);
            Assert.Equal(Color.FromArgb(255, 0xAA, 0xBB, 0xCC).ToArgb(), image.GetPixel(0, 0).ToArgb());
            Assert.Equal(0, image.GetPixel(1, 0).A);
        }

        [Fact]
        public void DecodePaletteWithAlphaTest()
        {
            var plte = new byte[] { 255, 0, 0, 0, 255, 0, 0, 0, 255 };
            var trns = new byte[] { 128 };
            var png = TestPng.Build(5, 2, 3, 2, false, (x, y) => new[] { (x + y) % 3 }, plte, trns);

            var image = CommonImage.FromPng(png);
            Assert.Equal(Color.FromArgb(128, 255, 0, 0).ToArgb(), image.GetPixel(0, 0).ToArgb());
            Assert.Equal(Color.FromArgb(255, 0, 255, 0).ToArgb(), image.GetPixel(1, 0).ToArgb());
            Assert.Equal(Color.FromArgb(255, 0, 0, 255).ToArgb(), image.GetPixel(1, 1).ToArgb());
        }

        [Fact]
        public void DecodeInterlacedTest()
        {
            Func<int, int, int[]> sample = (x, y) => new[] { x * 20, y * 20, (x * y) % 256, 255 - x - y };
            var interlaced = CommonImage.FromPng(TestPng.Build(13, 11, 6, 8, true, sample));
            var progressive = CommonImage.FromPng(TestPng.Build(13, 11, 6, 8, false, sample));

            AssertSamePixels(progressive, interlaced);
            Assert.Equal(Color.FromArgb(255 - 12 - 10, 240, 200, 120).ToArgb(), interlaced.GetPixel(12, 10).ToArgb());
        }

        [Fact]
        public void DecodeInterlacedTinyImageTest()
        {
            //1x1 的图片只有第一个子图有数据
            var image = CommonImage.FromPng(TestPng.Build(1, 1, 2, 8, true, (x, y) => new[] { 1, 2, 3 }));
            Assert.Equal(Color.FromArgb(255, 1, 2, 3).ToArgb(), image.GetPixel(0, 0).ToArgb());
        }

        [Fact]
        public void CorruptedPngTest()
        {
            var png = new CommonImage(4, 4, Color.White).ToPng();
            png[20] ^= 0xFF;

            Assert.Throws<InvalidDataException>(() => CommonImage.FromPng(png));
        }

        [Fact]
        public void JpegNotSupportedTest()
        {
            Assert.Throws<NotSupportedException>(() => CommonImage.FromPng(File.ReadAllBytes("TestData/001.jpg")));
        }

        private static void AssertSamePixels(CommonImage expected, CommonImage actual)
        {
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.Height, actual.Height);
            for (int y = 0; y < expected.Height; y++)
                for (int x = 0; x < expected.Width; x++)
                    Assert.Equal(expected.GetPixel(x, y).ToArgb(), actual.GetPixel(x, y).ToArgb());
        }

        /// <summary>
        /// 测试用的简易 PNG 编码器：不做行过滤，可生成任意颜色类型、位深和 Adam7 隔行的图片
        /// </summary>
        private static class TestPng
        {
            private static readonly int[][] Adam7 =
            {
                new[] { 0, 0, 8, 8 }, new[] { 4, 0, 8, 8 }, new[] { 0, 4, 4, 8 }, new[] { 2, 0, 4, 4 },
                new[] { 0, 2, 2, 4 }, new[] { 1, 0, 2, 2 }, new[] { 0, 1, 1, 2 },
            };

            public static byte[] Build(int width, int height, int colorType, int bitDepth, bool interlace,
                Func<int, int, int[]> sample, byte[] plte = null, byte[] trns = null)
            {
                var passes = interlace ? Adam7 : new[] { new[] { 0, 0, 1, 1 } };
                var raw = new MemoryStream();
                foreach (var p in passes)
                {
                    for (int y = p[1]; y < height; y += p[3])
                    {
                        var bits = new List<(int value, int width)>();
                        for (int x = p[0]; x < width; x += p[2])
                            foreach (var v in sample(x, y))
                                bits.Add((v, bitDepth));
                        if (bits.Count == 0)
                            continue;

                        raw.WriteByte(0);
                        raw.Write(Pack(bits));
                    }
                }

                var compressed = new MemoryStream();
                using (var z = new ZLibStream(compressed, CompressionLevel.Fastest, true))
                    z.Write(raw.ToArray());

                var ihdr = new byte[13];
                WriteInt32(ihdr, 0, width);
                WriteInt32(ihdr, 4, height);
                ihdr[8] = (byte)bitDepth;
                ihdr[9] = (byte)colorType;
                ihdr[12] = (byte)(interlace ? 1 : 0);

                var png = new MemoryStream();
                png.Write(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });
                Chunk(png, "IHDR", ihdr);
                if (plte != null)
                    Chunk(png, "PLTE", plte);
                if (trns != null)
                    Chunk(png, "tRNS", trns);
                Chunk(png, "IDAT", compressed.ToArray());
                Chunk(png, "IEND", Array.Empty<byte>());
                return png.ToArray();
            }

            private static byte[] Pack(List<(int value, int width)> samples)
            {
                var bytes = new List<byte>();
                int acc = 0, used = 0;
                foreach (var (value, width) in samples)
                {
                    if (width == 16)
                    {
                        bytes.Add((byte)(value >> 8));
                        bytes.Add((byte)value);
                        continue;
                    }
                    acc = (acc << width) | value;
                    used += width;
                    if (used == 8)
                    {
                        bytes.Add((byte)acc);
                        acc = used = 0;
                    }
                }
                if (used > 0)
                    bytes.Add((byte)(acc << (8 - used)));
                return bytes.ToArray();
            }

            private static void Chunk(Stream s, string type, byte[] data)
            {
                var head = new byte[8];
                WriteInt32(head, 0, data.Length);
                Encoding.ASCII.GetBytes(type, 0, 4, head, 4);
                s.Write(head);
                s.Write(data);

                var crcInput = new byte[4 + data.Length];
                Array.Copy(head, 4, crcInput, 0, 4);
                Array.Copy(data, 0, crcInput, 4, data.Length);
                var crc = new byte[4];
                WriteInt32(crc, 0, (int)Crc32(crcInput));
                s.Write(crc);
            }

            private static uint Crc32(byte[] data)
            {
                uint crc = 0xFFFFFFFF;
                foreach (var b in data)
                {
                    crc ^= b;
                    for (int k = 0; k < 8; k++)
                        crc = (crc & 1) != 0 ? 0xEDB88320 ^ (crc >> 1) : crc >> 1;
                }
                return crc ^ 0xFFFFFFFF;
            }

            private static void WriteInt32(byte[] data, int pos, int value)
            {
                data[pos] = (byte)(value >> 24);
                data[pos + 1] = (byte)(value >> 16);
                data[pos + 2] = (byte)(value >> 8);
                data[pos + 3] = (byte)value;
            }
        }
    }
}
