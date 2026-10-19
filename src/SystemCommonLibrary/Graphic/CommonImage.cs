using System;
using System.Drawing;
using System.IO;

namespace SystemCommonLibrary.Graphic
{
    /// <summary>
    /// 跨平台的 32 位 RGBA 位图，不依赖 System.Drawing.Common
    /// </summary>
    public sealed class CommonImage
    {
        /// <summary>
        /// 单张图片允许的最大像素数，防止恶意图片耗尽内存
        /// </summary>
        public const int MaxPixels = 1 << 28;

        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// 按行存储的像素数据，每像素 4 字节，顺序为 R、G、B、A（非预乘）
        /// </summary>
        internal byte[] Pixels { get; }

        public CommonImage(int width, int height)
        {
            if (width <= 0 || height <= 0 || (long)width * height > MaxPixels)
                throw new ArgumentOutOfRangeException(nameof(width), "图片尺寸无效");

            Width = width;
            Height = height;
            Pixels = new byte[width * height * 4];
        }

        public CommonImage(int width, int height, Color background) : this(width, height)
        {
            for (int i = 0; i < Pixels.Length; i += 4)
            {
                Pixels[i] = background.R;
                Pixels[i + 1] = background.G;
                Pixels[i + 2] = background.B;
                Pixels[i + 3] = background.A;
            }
        }

        public Color GetPixel(int x, int y)
        {
            int i = PixelIndex(x, y);
            return Color.FromArgb(Pixels[i + 3], Pixels[i], Pixels[i + 1], Pixels[i + 2]);
        }

        public void SetPixel(int x, int y, Color color)
        {
            int i = PixelIndex(x, y);
            Pixels[i] = color.R;
            Pixels[i + 1] = color.G;
            Pixels[i + 2] = color.B;
            Pixels[i + 3] = color.A;
        }

        /// <summary>
        /// 从 PNG 数据创建图片
        /// </summary>
        public static CommonImage FromPng(byte[] data)
        {
            return PngCodec.Decode(data);
        }

        public static CommonImage FromPng(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return PngCodec.Decode(ms.ToArray());
            }
        }

        /// <summary>
        /// 编码为 PNG，完全不透明的图片保存为 RGB，否则保存为 RGBA
        /// </summary>
        public byte[] ToPng()
        {
            using (var ms = new MemoryStream())
            {
                SavePng(ms);
                return ms.ToArray();
            }
        }

        public void SavePng(Stream stream)
        {
            PngCodec.Encode(this, stream);
        }

        /// <summary>
        /// 以 source-over 方式把颜色按覆盖率混合到指定像素，越界时忽略
        /// </summary>
        internal void Blend(int x, int y, byte r, byte g, byte b, byte a, float coverage)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height || coverage <= 0)
                return;

            float sa = a / 255f * System.Math.Min(coverage, 1f);
            if (sa <= 0)
                return;

            int i = (y * Width + x) * 4;
            float da = Pixels[i + 3] / 255f;
            float oa = sa + da * (1 - sa);
            if (oa <= 0)
                return;

            float dw = da * (1 - sa);
            Pixels[i] = ToByte((r * sa + Pixels[i] * dw) / oa);
            Pixels[i + 1] = ToByte((g * sa + Pixels[i + 1] * dw) / oa);
            Pixels[i + 2] = ToByte((b * sa + Pixels[i + 2] * dw) / oa);
            Pixels[i + 3] = ToByte(oa * 255);
        }

        private static byte ToByte(float value)
        {
            return (byte)System.Math.Clamp((int)System.Math.Round(value), 0, 255);
        }

        private int PixelIndex(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));

            return (y * Width + x) * 4;
        }
    }
}
