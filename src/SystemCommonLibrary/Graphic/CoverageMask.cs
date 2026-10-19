using System.Drawing;

namespace SystemCommonLibrary.Graphic
{
    /// <summary>
    /// 按面积计算覆盖率的抗锯齿蒙版：先累加所有矩形的覆盖率，再一次性混合到图片上，
    /// 避免相邻矩形在接缝处重复混合。像素 (x, y) 覆盖区域 [x-0.5, x+0.5] × [y-0.5, y+0.5]
    /// </summary>
    internal sealed class CoverageMask
    {
        private readonly int _x0, _y0, _width, _height;
        private readonly float[] _coverage;

        private CoverageMask(int x0, int y0, int width, int height)
        {
            _x0 = x0;
            _y0 = y0;
            _width = width;
            _height = height;
            _coverage = new float[width * height];
        }

        /// <summary>
        /// 创建覆盖指定区域的蒙版，区域与图片没有交集时返回 null
        /// </summary>
        public static CoverageMask Create(CommonImage image, float left, float top, float right, float bottom)
        {
            int x0 = System.Math.Max(0, FirstPixel(left));
            int y0 = System.Math.Max(0, FirstPixel(top));
            int x1 = System.Math.Min(image.Width - 1, LastPixel(right));
            int y1 = System.Math.Min(image.Height - 1, LastPixel(bottom));
            if (x1 < x0 || y1 < y0)
                return null;

            return new CoverageMask(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
        }

        /// <summary>
        /// 累加矩形的覆盖率，sign 为 -1 时扣除
        /// </summary>
        public void Add(float left, float top, float right, float bottom, float sign = 1)
        {
            int x0 = System.Math.Max(_x0, FirstPixel(left));
            int x1 = System.Math.Min(_x0 + _width - 1, LastPixel(right));
            int y0 = System.Math.Max(_y0, FirstPixel(top));
            int y1 = System.Math.Min(_y0 + _height - 1, LastPixel(bottom));

            for (int y = y0; y <= y1; y++)
            {
                float oy = Overlap(y, top, bottom);
                if (oy <= 0)
                    continue;

                for (int x = x0; x <= x1; x++)
                {
                    float ox = Overlap(x, left, right);
                    if (ox > 0)
                        _coverage[(y - _y0) * _width + (x - _x0)] += ox * oy * sign;
                }
            }
        }

        public void Apply(CommonImage image, Color color)
        {
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                {
                    float c = _coverage[y * _width + x];
                    if (c > 0)
                        image.Blend(_x0 + x, _y0 + y, color.R, color.G, color.B, color.A, c);
                }
        }

        private static float Overlap(int pixel, float start, float end)
        {
            return System.Math.Min(pixel + 0.5f, end) - System.Math.Max(pixel - 0.5f, start);
        }

        private static int FirstPixel(float start)
        {
            return (int)System.Math.Floor(start + 0.5f);
        }

        private static int LastPixel(float end)
        {
            return (int)System.Math.Ceiling(end + 0.5f) - 1;
        }
    }
}
