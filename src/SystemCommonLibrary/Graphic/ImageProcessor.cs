using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //graphics.CompositingMode = CompositingMode.SourceOver;

                var srcRect = new Rectangle(0, 0, watermark.Width, watermark.Height);
                var destRect = new Rectangle(bitmap.Width - watermark.Width, bitmap.Height - watermark.Height, watermark.Width, watermark.Height);
                graphics.DrawImage(watermark, destRect, srcRect, GraphicsUnit.Pixel);
                bitmap.Save(stream, ImageFormat.Jpeg);

                return stream;
            }
        }

        public static string StreamToBase64Image(MemoryStream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Close();

            return "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
        }
    }
}
