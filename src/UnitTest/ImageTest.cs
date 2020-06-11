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
            var ms = ImageProcessor.AddWatermark(img, "TUUTKU图库");
            string s = ImageProcessor.ImgToBase64String(ms);

            
        }
    }
}
