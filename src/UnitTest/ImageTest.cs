using System.IO;
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
    }
}
