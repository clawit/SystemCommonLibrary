using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SystemCommonLibrary.Graphic
{
    /// <summary>
    /// 纯托管的 PNG 编解码器，压缩使用 .NET 自带的 ZLibStream
    /// </summary>
    internal static class PngCodec
    {
        private static readonly byte[] Signature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        // Adam7 隔行扫描的 7 个子图：起始列、起始行、列间隔、行间隔
        private static readonly int[][] Adam7 =
        {
            new[] { 0, 0, 8, 8 },
            new[] { 4, 0, 8, 8 },
            new[] { 0, 4, 4, 8 },
            new[] { 2, 0, 4, 4 },
            new[] { 0, 2, 2, 4 },
            new[] { 1, 0, 2, 2 },
            new[] { 0, 1, 1, 2 },
        };

        private static readonly uint[] CrcTable = CreateCrcTable();

        public static bool IsPng(byte[] data)
        {
            if (data == null || data.Length < Signature.Length)
                return false;

            for (int i = 0; i < Signature.Length; i++)
            {
                if (data[i] != Signature[i])
                    return false;
            }
            return true;
        }

        #region 解码

        public static CommonImage Decode(byte[] data)
        {
            if (!IsPng(data))
                throw new NotSupportedException("不支持的格式类型，仅支持 PNG 图片");

            int width = 0, height = 0, bitDepth = 0, colorType = 0, interlace = 0;
            bool hasHeader = false;
            byte[] palette = null;
            byte[] paletteAlpha = null;
            int[] transparentColor = null;
            var idat = new MemoryStream();

            int pos = Signature.Length;
            while (true)
            {
                if (pos + 12 > data.Length)
                    throw new InvalidDataException("PNG 数据不完整");

                int length = ReadInt32(data, pos);
                if (length < 0 || pos + 12L + length > data.Length)
                    throw new InvalidDataException("PNG 数据块长度无效");

                string type = Encoding.ASCII.GetString(data, pos + 4, 4);
                int start = pos + 8;
                uint crc = (uint)ReadInt32(data, start + length);
                if (crc != Crc32(data, pos + 4, length + 4))
                    throw new InvalidDataException($"PNG 数据块 {type} 校验失败");
                pos = start + length + 4;

                if (type == "IHDR")
                {
                    if (length != 13)
                        throw new InvalidDataException("PNG 文件头无效");

                    width = ReadInt32(data, start);
                    height = ReadInt32(data, start + 4);
                    bitDepth = data[start + 8];
                    colorType = data[start + 9];
                    interlace = data[start + 12];
                    if (data[start + 10] != 0 || data[start + 11] != 0 || interlace > 1)
                        throw new InvalidDataException("不支持的 PNG 压缩、过滤或隔行方式");
                    if (!IsValidFormat(colorType, bitDepth))
                        throw new InvalidDataException($"无效的 PNG 颜色类型 {colorType} 与位深 {bitDepth}");
                    if (width <= 0 || height <= 0 || (long)width * height > CommonImage.MaxPixels)
                        throw new InvalidDataException("PNG 图片尺寸无效");
                    hasHeader = true;
                }
                else if (!hasHeader)
                {
                    throw new InvalidDataException("PNG 缺少文件头");
                }
                else if (type == "PLTE")
                {
                    if (length % 3 != 0 || length == 0 || length > 256 * 3)
                        throw new InvalidDataException("PNG 调色板无效");

                    palette = new byte[length];
                    Array.Copy(data, start, palette, 0, length);
                }
                else if (type == "tRNS")
                {
                    if (colorType == 3)
                    {
                        paletteAlpha = new byte[length];
                        Array.Copy(data, start, paletteAlpha, 0, length);
                    }
                    else if (colorType == 0 && length >= 2)
                    {
                        transparentColor = new[] { ReadUInt16(data, start) };
                    }
                    else if (colorType == 2 && length >= 6)
                    {
                        transparentColor = new[] { ReadUInt16(data, start), ReadUInt16(data, start + 2), ReadUInt16(data, start + 4) };
                    }
                }
                else if (type == "IDAT")
                {
                    idat.Write(data, start, length);
                }
                else if (type == "IEND")
                {
                    break;
                }
                else if (char.IsUpper(type[0]))
                {
                    // 类型首字母大写表示关键数据块，无法识别时不能忽略
                    throw new NotSupportedException($"不支持的 PNG 数据块 {type}");
                }
            }

            if (colorType == 3 && palette == null)
                throw new InvalidDataException("PNG 缺少调色板");

            int channels = Channels(colorType);
            int bitsPerPixel = channels * bitDepth;
            int filterUnit = System.Math.Max(1, bitsPerPixel / 8);

            // 计算解压后应有的数据量
            long expected = 0;
            foreach (var pass in Passes(interlace, width, height))
            {
                expected += (StrideOf(pass[4], bitsPerPixel) + 1L) * pass[5];
            }
            if (expected > int.MaxValue)
                throw new InvalidDataException("PNG 图片尺寸过大");

            var raw = new byte[expected];
            idat.Position = 0;
            using (var zlib = new ZLibStream(idat, CompressionMode.Decompress))
            {
                int read = 0;
                while (read < raw.Length)
                {
                    int n = zlib.Read(raw, read, raw.Length - read);
                    if (n == 0)
                        throw new InvalidDataException("PNG 图像数据不完整");
                    read += n;
                }
            }

            var image = new CommonImage(width, height);
            int offset = 0;
            foreach (var pass in Passes(interlace, width, height))
            {
                int passWidth = pass[4], passHeight = pass[5];
                int stride = StrideOf(passWidth, bitsPerPixel);
                var prior = new byte[stride];
                var line = new byte[stride];

                for (int y = 0; y < passHeight; y++)
                {
                    byte filter = raw[offset];
                    Array.Copy(raw, offset + 1, line, 0, stride);
                    offset += stride + 1;
                    Unfilter(filter, line, prior, filterUnit);

                    int destY = pass[1] + y * pass[3];
                    for (int x = 0; x < passWidth; x++)
                    {
                        int destX = pass[0] + x * pass[2];
                        WritePixel(image, destX, destY, line, x, colorType, bitDepth, palette, paletteAlpha, transparentColor);
                    }

                    var swap = prior;
                    prior = line;
                    line = swap;
                }
            }

            return image;
        }

        private static void Unfilter(byte filter, byte[] line, byte[] prior, int unit)
        {
            switch (filter)
            {
                case 0:
                    break;
                case 1:
                    for (int i = unit; i < line.Length; i++)
                        line[i] += line[i - unit];
                    break;
                case 2:
                    for (int i = 0; i < line.Length; i++)
                        line[i] += prior[i];
                    break;
                case 3:
                    for (int i = 0; i < line.Length; i++)
                    {
                        int left = i >= unit ? line[i - unit] : 0;
                        line[i] += (byte)((left + prior[i]) >> 1);
                    }
                    break;
                case 4:
                    for (int i = 0; i < line.Length; i++)
                    {
                        int left = i >= unit ? line[i - unit] : 0;
                        int upLeft = i >= unit ? prior[i - unit] : 0;
                        line[i] += Paeth(left, prior[i], upLeft);
                    }
                    break;
                default:
                    throw new InvalidDataException($"无效的 PNG 过滤类型 {filter}");
            }
        }

        private static void WritePixel(CommonImage image, int x, int y, byte[] line, int index,
            int colorType, int bitDepth, byte[] palette, byte[] paletteAlpha, int[] transparentColor)
        {
            byte r, g, b, a = 255;
            switch (colorType)
            {
                case 0:
                    {
                        int v = Sample(line, index, bitDepth);
                        r = g = b = ScaleTo8(v, bitDepth);
                        if (transparentColor != null && v == transparentColor[0])
                            a = 0;
                        break;
                    }
                case 2:
                    {
                        int vr = Sample(line, index * 3, bitDepth);
                        int vg = Sample(line, index * 3 + 1, bitDepth);
                        int vb = Sample(line, index * 3 + 2, bitDepth);
                        r = ScaleTo8(vr, bitDepth);
                        g = ScaleTo8(vg, bitDepth);
                        b = ScaleTo8(vb, bitDepth);
                        if (transparentColor != null && vr == transparentColor[0] && vg == transparentColor[1] && vb == transparentColor[2])
                            a = 0;
                        break;
                    }
                case 3:
                    {
                        int v = Sample(line, index, bitDepth);
                        if (v * 3 + 2 >= palette.Length)
                            throw new InvalidDataException("PNG 调色板索引越界");
                        r = palette[v * 3];
                        g = palette[v * 3 + 1];
                        b = palette[v * 3 + 2];
                        if (paletteAlpha != null && v < paletteAlpha.Length)
                            a = paletteAlpha[v];
                        break;
                    }
                case 4:
                    r = g = b = ScaleTo8(Sample(line, index * 2, bitDepth), bitDepth);
                    a = ScaleTo8(Sample(line, index * 2 + 1, bitDepth), bitDepth);
                    break;
                default:
                    r = ScaleTo8(Sample(line, index * 4, bitDepth), bitDepth);
                    g = ScaleTo8(Sample(line, index * 4 + 1, bitDepth), bitDepth);
                    b = ScaleTo8(Sample(line, index * 4 + 2, bitDepth), bitDepth);
                    a = ScaleTo8(Sample(line, index * 4 + 3, bitDepth), bitDepth);
                    break;
            }

            int i = (y * image.Width + x) * 4;
            image.Pixels[i] = r;
            image.Pixels[i + 1] = g;
            image.Pixels[i + 2] = b;
            image.Pixels[i + 3] = a;
        }

        /// <summary>
        /// 读取一行中第 n 个采样值
        /// </summary>
        private static int Sample(byte[] line, int n, int bitDepth)
        {
            switch (bitDepth)
            {
                case 8:
                    return line[n];
                case 16:
                    return (line[n * 2] << 8) | line[n * 2 + 1];
                default:
                    int bit = n * bitDepth;
                    int shift = 8 - bitDepth - (bit & 7);
                    return (line[bit >> 3] >> shift) & ((1 << bitDepth) - 1);
            }
        }

        private static byte ScaleTo8(int value, int bitDepth)
        {
            switch (bitDepth)
            {
                case 8:
                    return (byte)value;
                case 16:
                    return (byte)(value >> 8);
                default:
                    return (byte)(value * 255 / ((1 << bitDepth) - 1));
            }
        }

        private static bool IsValidFormat(int colorType, int bitDepth)
        {
            switch (colorType)
            {
                case 0:
                    return bitDepth == 1 || bitDepth == 2 || bitDepth == 4 || bitDepth == 8 || bitDepth == 16;
                case 3:
                    return bitDepth == 1 || bitDepth == 2 || bitDepth == 4 || bitDepth == 8;
                case 2:
                case 4:
                case 6:
                    return bitDepth == 8 || bitDepth == 16;
                default:
                    return false;
            }
        }

        private static int Channels(int colorType)
        {
            switch (colorType)
            {
                case 2: return 3;
                case 4: return 2;
                case 6: return 4;
                default: return 1;
            }
        }

        /// <summary>
        /// 返回每个子图的 起始列、起始行、列间隔、行间隔、宽、高，跳过空子图
        /// </summary>
        private static IEnumerable<int[]> Passes(int interlace, int width, int height)
        {
            if (interlace == 0)
            {
                yield return new[] { 0, 0, 1, 1, width, height };
                yield break;
            }

            foreach (var p in Adam7)
            {
                int w = (width - p[0] + p[2] - 1) / p[2];
                int h = (height - p[1] + p[3] - 1) / p[3];
                if (w > 0 && h > 0)
                    yield return new[] { p[0], p[1], p[2], p[3], w, h };
            }
        }

        private static int StrideOf(int width, int bitsPerPixel)
        {
            return (int)(((long)width * bitsPerPixel + 7) / 8);
        }

        #endregion

        #region 编码

        public static void Encode(CommonImage image, Stream output)
        {
            var pixels = image.Pixels;
            bool opaque = true;
            for (int i = 3; i < pixels.Length; i += 4)
            {
                if (pixels[i] != 255)
                {
                    opaque = false;
                    break;
                }
            }

            int channels = opaque ? 3 : 4;
            int stride = image.Width * channels;

            var header = new byte[13];
            WriteInt32(header, 0, image.Width);
            WriteInt32(header, 4, image.Height);
            header[8] = 8;
            header[9] = (byte)(opaque ? 2 : 6);

            var compressed = new MemoryStream();
            using (var zlib = new ZLibStream(compressed, CompressionLevel.Optimal, true))
            {
                var prior = new byte[stride];
                var line = new byte[stride];
                var filtered = new byte[5][];
                for (int f = 0; f < filtered.Length; f++)
                    filtered[f] = new byte[stride + 1];

                for (int y = 0; y < image.Height; y++)
                {
                    int src = y * image.Width * 4;
                    for (int x = 0; x < image.Width; x++)
                    {
                        Array.Copy(pixels, src + x * 4, line, x * channels, channels);
                    }

                    zlib.Write(ChooseFilter(line, prior, channels, filtered));

                    var swap = prior;
                    prior = line;
                    line = swap;
                }
            }

            output.Write(Signature, 0, Signature.Length);
            WriteChunk(output, "IHDR", header, header.Length);
            WriteChunk(output, "IDAT", compressed.GetBuffer(), (int)compressed.Length);
            WriteChunk(output, "IEND", Array.Empty<byte>(), 0);
        }

        /// <summary>
        /// 对一行尝试全部 5 种过滤方式，取差值绝对值之和最小的一种（PNG 规范推荐的启发式）
        /// </summary>
        private static byte[] ChooseFilter(byte[] line, byte[] prior, int unit, byte[][] filtered)
        {
            byte[] best = null;
            long bestSum = long.MaxValue;
            for (int f = 0; f < filtered.Length; f++)
            {
                var buf = filtered[f];
                buf[0] = (byte)f;
                long sum = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    int left = i >= unit ? line[i - unit] : 0;
                    int up = prior[i];
                    int upLeft = i >= unit ? prior[i - unit] : 0;
                    byte predictor;
                    switch (f)
                    {
                        case 1: predictor = (byte)left; break;
                        case 2: predictor = (byte)up; break;
                        case 3: predictor = (byte)((left + up) >> 1); break;
                        case 4: predictor = Paeth(left, up, upLeft); break;
                        default: predictor = 0; break;
                    }
                    byte v = (byte)(line[i] - predictor);
                    buf[i + 1] = v;
                    sum += v < 128 ? v : 256 - v;
                }

                if (sum < bestSum)
                {
                    bestSum = sum;
                    best = buf;
                }
            }
            return best;
        }

        private static void WriteChunk(Stream output, string type, byte[] data, int length)
        {
            var head = new byte[8];
            WriteInt32(head, 0, length);
            Encoding.ASCII.GetBytes(type, 0, 4, head, 4);
            output.Write(head, 0, head.Length);
            output.Write(data, 0, length);

            uint crc = UpdateCrc(0xFFFFFFFF, head, 4, 4);
            crc = UpdateCrc(crc, data, 0, length) ^ 0xFFFFFFFF;
            var tail = new byte[4];
            WriteInt32(tail, 0, (int)crc);
            output.Write(tail, 0, tail.Length);
        }

        #endregion

        private static byte Paeth(int a, int b, int c)
        {
            int p = a + b - c;
            int pa = System.Math.Abs(p - a);
            int pb = System.Math.Abs(p - b);
            int pc = System.Math.Abs(p - c);
            if (pa <= pb && pa <= pc)
                return (byte)a;
            return pb <= pc ? (byte)b : (byte)c;
        }

        private static int ReadInt32(byte[] data, int pos)
        {
            return (data[pos] << 24) | (data[pos + 1] << 16) | (data[pos + 2] << 8) | data[pos + 3];
        }

        private static int ReadUInt16(byte[] data, int pos)
        {
            return (data[pos] << 8) | data[pos + 1];
        }

        private static void WriteInt32(byte[] data, int pos, int value)
        {
            data[pos] = (byte)(value >> 24);
            data[pos + 1] = (byte)(value >> 16);
            data[pos + 2] = (byte)(value >> 8);
            data[pos + 3] = (byte)value;
        }

        private static uint Crc32(byte[] data, int offset, int length)
        {
            return UpdateCrc(0xFFFFFFFF, data, offset, length) ^ 0xFFFFFFFF;
        }

        private static uint UpdateCrc(uint crc, byte[] data, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
                crc = CrcTable[(crc ^ data[i]) & 0xFF] ^ (crc >> 8);
            return crc;
        }

        private static uint[] CreateCrcTable()
        {
            var table = new uint[256];
            for (uint n = 0; n < 256; n++)
            {
                uint c = n;
                for (int k = 0; k < 8; k++)
                    c = (c & 1) != 0 ? 0xEDB88320 ^ (c >> 1) : c >> 1;
                table[n] = c;
            }
            return table;
        }
    }
}
