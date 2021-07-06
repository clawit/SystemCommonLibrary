using System.Drawing;
using System.IO;
using SystemCommonLibrary.Encrypt;
using SystemCommonLibrary.Graphic;
using Xunit;

namespace UnitTest
{
    public class ImageTest
    {
        [Fact]
        public void Image2Base64Test()
        {
            var img = ImageProcessor.ReadFromFile("TestData/001.jpg");
            var watermark = ImageProcessor.ReadFromFile("TestData/watermark.png");
            var ms = ImageProcessor.AddWatermark(img, watermark);
            string s = ImageProcessor.StreamToBase64Image(ms, img.RawFormat);

            Assert.Equal(File.ReadAllText("TestData/base64Image.txt"), s);
        }

        [Fact]
        public void GenerateVerificationTest()
        {
            //生成4位验证码
            var code = CodeGenerator.RndHash().Substring(0, 4);

            //渲染画布
            string sOrignal = "iVBORw0KGgoAAAANSUhEUgAAAGQAAAAeCAYAAADaW7vzAAABRGlDQ1BJQ0MgUHJvZmlsZQAAKJFjYGASSSwoyGFhYGDIzSspCnJ3UoiIjFJgf8rAzsDGIMggzcCSmFxc4BgQ4ANUwgCjUcG3awyMIPqyLsismr8dFYcllqguTZDMtDz9QxFTPQrgSkktTgbSf4A4LbmgqISBgTEFyFYuLykAsTuAbJEioKOA7DkgdjqEvQHEToKwj4DVhAQ5A9k3gGyB5IxEoBmML4BsnSQk8XQkNtReEOBxcfXxUQgwMjK09CDgXNJBSWpFCYh2zi+oLMpMzyhRcASGUqqCZ16yno6CkYGRIQMDKMwhqj/fAIcloxgHQizZlYHBKAnIOI8Qy+piYNgtCvSGBUJMk52BQQgofki1ILEoEe4Axm8sxWnGRhA293YGBtZp//9/DmdgYNdkYPh7/f//39v///+7jIGB+RYDw4FvADfyXfRxLn+pAAAAOGVYSWZNTQAqAAAACAABh2kABAAAAAEAAAAaAAAAAAACoAIABAAAAAEAAABkoAMABAAAAAEAAAAeAAAAAL8RBBUAAACHSURBVGgF7dNBDQAhEMXQZf2rBHxAgop36Cho2vljrn2+jjHwMySBPAMFwR6hIAXBDGA4LaQgmAEMp4UUBDOA4bSQgmAGMJwWUhDMAIbTQgqCGcBwWkhBMAMYTgspCGYAw2khBcEMYDgtpCCYAQynhRQEM4DhtJCCYAYwnBZSEMwAhtNCsCAXgoED5Hj+2+cAAAAASUVORK5CYII=";
            var image = ImageProcessor.Base64ToImage(sOrignal);
            ImageProcessor.DrawText(image, code.Substring(0, 1), new PointF(5, 5));
            ImageProcessor.DrawText(image, code.Substring(1, 1), new PointF(30, 5));
            ImageProcessor.DrawText(image, code.Substring(2, 1), new PointF(55, 5));
            ImageProcessor.DrawText(image, code.Substring(3, 1), new PointF(75, 5));

            //返回base64
            string sImg = ImageProcessor.ImageToBase64Image(image);
            Assert.NotEmpty(sImg);
        }


    }
}
