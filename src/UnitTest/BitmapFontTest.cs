using System.Drawing;
using SystemCommonLibrary.Graphic;
using Xunit;

namespace UnitTest
{
    public class BitmapFontTest
    {
        [Fact]
        public void AllPrintableAsciiDefinedTest()
        {
            for (char c = ' '; c <= '~'; c++)
                Assert.True(BitmapFont.Contains(c), $"缺少字符 {c}");
        }

        [Fact]
        public void DrawTextInsideLayoutBoxTest()
        {
            var image = new CommonImage(40, 40, Color.White);
            BitmapFont.DrawText(image, "H", 5, 5, 12, Color.Black);

            //12 磅 = 16 像素，点距 1.6 像素，字形区域约为 x 6.6~14.6、y 8.2~19.4
            int inked = 0;
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    if (image.GetPixel(x, y).ToArgb() == Color.White.ToArgb())
                        continue;

                    inked++;
                    Assert.InRange(x, 6, 15);
                    Assert.InRange(y, 8, 20);
                }
            Assert.True(inked > 20);
            Assert.Equal(Color.Black.ToArgb(), image.GetPixel(10, 14).ToArgb());
        }

        [Fact]
        public void DrawTextClippedTest()
        {
            var image = new CommonImage(10, 10, Color.White);
            BitmapFont.DrawText(image, "ABC", -8, -8, 24, Color.Black);
            BitmapFont.DrawText(image, "ABC", 100, 100, 24, Color.Black);
        }

        [Fact]
        public void UnsupportedCharDrawsBoxTest()
        {
            Assert.False(BitmapFont.Contains('中'));
            Assert.Equal(BitmapFont.GetGlyph('中'), BitmapFont.GetGlyph('é'));
            Assert.NotEqual(BitmapFont.GetGlyph('中'), BitmapFont.GetGlyph('A'));
            Assert.Equal(0b11111, BitmapFont.GetGlyph('中')[0]);
        }
    }
}
