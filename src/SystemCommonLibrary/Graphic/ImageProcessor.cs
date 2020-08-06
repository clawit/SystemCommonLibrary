using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Configuration;
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
                bitmap.Save(stream, bitmap.RawFormat);

                return stream;
            }
        }

        public static string ImageToBase64Image(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return StreamToBase64Image(ms, img.RawFormat);
            }
        }

        public static string StreamToBase64Image(MemoryStream stream, ImageFormat imageFormat)
        {
            string prefix = string.Empty;
            if (imageFormat.Equals(ImageFormat.Jpeg))
            {
                prefix = "data:image/jpeg;base64,";
            }
            else if (imageFormat.Equals(ImageFormat.Png))
            {
                prefix = "data:image/png;base64,";
            }
            else
                throw new NotSupportedException("不支持的格式类型");

            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Close();

            return prefix + Convert.ToBase64String(bytes);
        }

        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(ms, true);

            return image;
        }

        public static Image DrawRect(Image image, Rectangle rect, Pen pen = null)
        {
            if (pen == null)
            {
                pen = new Pen(Color.Gray, 2);
            }

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawRectangle(pen, rect);
            }

            return image;
        }

        public static Image DrawText(Image image, string text, PointF point, Font font = null, Brush brush = null)
        {
            if (font == null)
            {
                font = new Font("Arial", 12);
            }
            if (brush == null)
            {
                brush = new SolidBrush(Color.Gray);
            }
            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawString(text, font, brush, point);
            }

            return image;
        }
    }
}
