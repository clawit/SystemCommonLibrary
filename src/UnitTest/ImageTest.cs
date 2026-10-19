using System;
using System.Drawing;
using System.IO;
using SystemCommonLibrary.Encrypt;
using SystemCommonLibrary.Graphic;
using Xunit;

namespace UnitTest
{
    public class ImageTest
    {
        private const string PngPrefix = "data:image/png;base64,";

        [Fact]
        public void Image2Base64Test()
        {
            var background = Color.FromArgb(255, 20, 60, 100);
            var img = new CommonImage(500, 300, background);
            var watermark = ImageProcessor.ReadFromFile("TestData/watermark.png");
            var ms = ImageProcessor.AddWatermark(img, watermark);
            string s = ImageProcessor.StreamToBase64Image(ms, ImageFormat.Png);

            Assert.StartsWith(PngPrefix, s);
            var result = ImageProcessor.Base64ToImage(s.Substring(PngPrefix.Length));
            Assert.Equal(500, result.Width);
            Assert.Equal(300, result.Height);
            Assert.Equal(background.ToArgb(), result.GetPixel(0, 0).ToArgb());

            //水印按透明度叠加在底图的右下角
            int left = 500 - watermark.Width, top = 300 - watermark.Height;
            int blended = 0;
            for (int y = 0; y < watermark.Height; y++)
                for (int x = 0; x < watermark.Width; x++)
                {
                    var c = watermark.GetPixel(x, y);
                    var actual = result.GetPixel(left + x, top + y);
                    float a = c.A / 255f;
                    Assert.Equal(255, actual.A);
                    Assert.InRange(actual.R, Over(c.R, background.R, a) - 1, Over(c.R, background.R, a) + 1);
                    Assert.InRange(actual.G, Over(c.G, background.G, a) - 1, Over(c.G, background.G, a) + 1);
                    Assert.InRange(actual.B, Over(c.B, background.B, a) - 1, Over(c.B, background.B, a) + 1);
                    if (actual.ToArgb() != background.ToArgb())
                        blended++;
                }
            Assert.True(blended > 0);
        }

        private static int Over(int value, int background, float alpha)
        {
            return (int)Math.Round(value * alpha + background * (1 - alpha));
        }

        [Fact]
        public void GenerateVerificationTest()
        {
            //生成4位验证码
            var code = CodeGenerator.RndHash().Substring(0, 4);

            //渲染画布
            string sOrignal = "iVBORw0KGgoAAAANSUhEUgAAAGQAAAAeCAYAAADaW7vzAAABRGlDQ1BJQ0MgUHJvZmlsZQAAKJFjYGASSSwoyGFhYGDIzSspCnJ3UoiIjFJgf8rAzsDGIMggzcCSmFxc4BgQ4ANUwgCjUcG3awyMIPqyLsismr8dFYcllqguTZDMtDz9QxFTPQrgSkktTgbSf4A4LbmgqISBgTEFyFYuLykAsTuAbJEioKOA7DkgdjqEvQHEToKwj4DVhAQ5A9k3gGyB5IxEoBmML4BsnSQk8XQkNtReEOBxcfXxUQgwMjK09CDgXNJBSWpFCYh2zi+oLMpMzyhRcASGUqqCZ16yno6CkYGRIQMDKMwhqj/fAIcloxgHQizZlYHBKAnIOI8Qy+piYNgtCvSGBUJMk52BQQgofki1ILEoEe4Axm8sxWnGRhA293YGBtZp//9/DmdgYNdkYPh7/f//39v///+7jIGB+RYDw4FvADfyXfRxLn+pAAAAOGVYSWZNTQAqAAAACAABh2kABAAAAAEAAAAaAAAAAAACoAIABAAAAAEAAABkoAMABAAAAAEAAAAeAAAAAL8RBBUAAACHSURBVGgF7dNBDQAhEMXQZf2rBHxAgop36Cho2vljrn2+jjHwMySBPAMFwR6hIAXBDGA4LaQgmAEMp4UUBDOA4bSQgmAGMJwWUhDMAIbTQgqCGcBwWkhBMAMYTgspCGYAw2khBcEMYDgtpCCYAQynhRQEM4DhtJCCYAYwnBZSEMwAhtNCsCAXgoED5Hj+2+cAAAAASUVORK5CYII=";
            var image = ImageProcessor.Base64ToImage(sOrignal);
            var background = ImageProcessor.Base64ToImage(sOrignal);
            ImageProcessor.DrawText(image, code.Substring(0, 1), new PointF(5, 5));
            ImageProcessor.DrawText(image, code.Substring(1, 1), new PointF(30, 5));
            ImageProcessor.DrawText(image, code.Substring(2, 1), new PointF(55, 5));
            ImageProcessor.DrawText(image, code.Substring(3, 1), new PointF(75, 5));

            //返回base64
            string sImg = ImageProcessor.ImageToBase64Image(image);
            Assert.StartsWith(PngPrefix, sImg);

            var result = ImageProcessor.Base64ToImage(sImg.Substring(PngPrefix.Length));
            Assert.Equal(100, result.Width);
            Assert.Equal(30, result.Height);

            //四个字符各自所在的区域都画上了文字
            foreach (var x0 in new[] { 5, 30, 55, 75 })
            {
                int changed = 0;
                for (int y = 0; y < 30; y++)
                    for (int x = x0; x < x0 + 12; x++)
                    {
                        if (result.GetPixel(x, y) != background.GetPixel(x, y))
                            changed++;
                    }
                Assert.True(changed > 5, $"x={x0} 处没有绘制文字");
            }
        }

        [Fact]
        public void DrawRectTest()
        {
            var image = new CommonImage(20, 20, Color.White);
            ImageProcessor.DrawRect(image, new Rectangle(5, 5, 10, 10), Color.Red, 1);

            Assert.Equal(Color.Red.ToArgb(), image.GetPixel(5, 5).ToArgb());
            Assert.Equal(Color.Red.ToArgb(), image.GetPixel(10, 5).ToArgb());
            Assert.Equal(Color.Red.ToArgb(), image.GetPixel(15, 15).ToArgb());
            Assert.Equal(Color.White.ToArgb(), image.GetPixel(10, 10).ToArgb());
            Assert.Equal(Color.White.ToArgb(), image.GetPixel(2, 2).ToArgb());
        }

        [Fact]
        public void ReadJpegNotSupportedTest()
        {
            Assert.Throws<NotSupportedException>(() => ImageProcessor.ReadFromFile("TestData/001.jpg"));
        }
    }
}
