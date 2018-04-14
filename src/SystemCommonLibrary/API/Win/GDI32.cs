using System;
using System.Runtime.InteropServices;

namespace SystemCommonLibrary.API.Win
{
    /// <summary>
    /// Helper class containing Gdi32 API functions
    /// </summary>
    public class GDI32
    {
        private const string GDI32_DLL = "GDI32.DLL";

        //BitBlt dwRop parameter
        public enum dwRop : int
        {
            SRCAND = 0x008800C6,
            SRCCOPY = 0x00CC0020,
            SRCINVERT = 0x00660046,
            SRCPAINT = 0x00EE0086
        }

        //GetCurrentObject uObjectType parameter
        public enum ObjectType : uint
        {
            OBJ_PEN = 0x0001,
            OBJ_BRUSH = 0x0002,
            OBJ_PAL = 0x0005,
            OBJ_FONT = 0x0006,
            OBJ_BITMAP = 0x0007
        }

        [DllImport(GDI32_DLL)]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, dwRop dwRop);

        [DllImport(GDI32_DLL)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, 
            int nWidth, int nHeight);

        [DllImport(GDI32_DLL)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport(GDI32_DLL)]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport(GDI32_DLL)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(GDI32_DLL)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport(GDI32_DLL)]
        public static extern IntPtr GetCurrentObject(IntPtr hDC, uint uObjectType);

        [DllImport(GDI32_DLL)]
        public static extern bool MoveToEx(IntPtr hDC, int x, int y, ref IntPtr lpPoint);

        [DllImport(GDI32_DLL)]
        public static extern bool LineTo(IntPtr hDC, int x, int y);

        [DllImport(GDI32_DLL)]
        public static extern bool TextOut(IntPtr hDC, int x, int y, string str, int length);

    }
}