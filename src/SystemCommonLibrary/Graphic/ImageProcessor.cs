using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Graphic
{
    public static class ImageProcessor
    {
        public static Image ReadFromFile(string filename)
        {
            return new Bitmap(filename);        
        }

        public static MemoryStream AddWatermark(Image bitmap, Image watermark)
        {
            var stream = new MemoryStream();
            var image = new Bitmap(bitmap);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;

                var pos = new Point(image.Width - watermark.Width, image.Height - watermark.Height);

                graphics.DrawImageUnscaled(watermark, pos);
                image.Save(stream, ImageFormat.Jpeg);

                return stream;
            }
        }

        public static string ImgToBase64String(MemoryStream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Close();

            return "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
        }
    }
}
