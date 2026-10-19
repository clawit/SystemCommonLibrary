using System;
using System.Drawing;
using System.IO;

namespace SystemCommonLibrary.Graphic
{
    /// <summary>
    /// 跨平台的图片处理，仅支持 PNG 格式
    /// </summary>
    public static class ImageProcessor
    {
        public static CommonImage ReadFromFile(string filename)
        {
            return CommonImage.FromPng(File.ReadAllBytes(filename));
        }

        /// <summary>
        /// 把水印绘制到图片右下角，返回 PNG 数据流
        /// </summary>
        public static MemoryStream AddWatermark(CommonImage bitmap, CommonImage watermark)
        {
            int left = bitmap.Width - watermark.Width;
            int top = bitmap.Height - watermark.Height;
            for (int y = 0; y < watermark.Height; y++)
                for (int x = 0; x < watermark.Width; x++)
                {
                    var c = watermark.GetPixel(x, y);
                    bitmap.Blend(left + x, top + y, c.R, c.G, c.B, c.A, 1);
                }

            var stream = new MemoryStream();
            bitmap.SavePng(stream);
            return stream;
        }

        public static string ImageToBase64Image(CommonImage img)
        {
            using (var ms = new MemoryStream())
            {
                img.SavePng(ms);
                return StreamToBase64Image(ms, ImageFormat.Png);
            }
        }

        public static string StreamToBase64Image(MemoryStream stream, ImageFormat imageFormat)
        {
            string prefix = string.Empty;
            if (imageFormat == ImageFormat.Jpeg)
            {
                prefix = "data:image/jpeg;base64,";
            }
            else if (imageFormat == ImageFormat.Png)
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

        public static CommonImage Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            return CommonImage.FromPng(imageBytes);
        }

        /// <summary>
        /// 绘制矩形边框，线条以矩形边为中心，默认灰色、宽 2 像素
        /// </summary>
        public static CommonImage DrawRect(CommonImage image, Rectangle rect, Color? color = null, float width = 2)
        {
            var c = color ?? Color.Gray;
            float half = width / 2;
            var mask = CoverageMask.Create(image, rect.Left - half, rect.Top - half, rect.Right + half, rect.Bottom + half);
            if (mask == null)
                return image;

            mask.Add(rect.Left - half, rect.Top - half, rect.Right + half, rect.Bottom + half);
            if (rect.Width > width && rect.Height > width)
                mask.Add(rect.Left + half, rect.Top + half, rect.Right - half, rect.Bottom - half, -1);
            mask.Apply(image, c);

            return image;
        }

        /// <summary>
        /// 使用内置点阵字体绘制文字，仅支持可打印 ASCII 字符，其他字符显示为方框。
        /// 字号单位为磅，默认灰色 12 磅
        /// </summary>
        public static CommonImage DrawText(CommonImage image, string text, PointF point, float fontSize = 12, Color? color = null)
        {
            BitmapFont.DrawText(image, text, point.X, point.Y, fontSize, color ?? Color.Gray);

            return image;
        }
    }
}
