using System;
using System.Collections.Generic;
using SystemCommonLibrary.Attribute;
using SystemCommonLibrary.Graphic;
using SystemCommonLibrary.Serialization;
using Xunit;

namespace UnitTest
{
    public class ImageTest
    {
        [Fact]
        public void Image2Base64Test()
        {
            var img = ImageProcessor.ReadFromFile("TestData/mount.jpg");
            var watermark = ImageProcessor.ReadFromFile("TestData/avt.png");
            var ms = ImageProcessor.AddWatermark(img, watermark);
            string s = ImageProcessor.ImgToBase64String(ms);

            Assert.True(true);
        }
    }
}
