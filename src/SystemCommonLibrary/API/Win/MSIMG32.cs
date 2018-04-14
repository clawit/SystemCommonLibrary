using System;
using System.Runtime.InteropServices;

namespace SystemCommonLibrary.API.Win
{


	public class MSIMG32
	{
		private const string MSIMG32_DLL = "MSIMG32.DLL";

		[DllImport (MSIMG32_DLL)]
		public static extern bool TransparentBlt (
			IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int hHeightDest,
			IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
			uint crTransparent);

		[DllImport (MSIMG32_DLL)]
		public static extern bool AlphaBlend (
			IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int hHeightDest,
			IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
			BlendFunction ftn);
	}
}
