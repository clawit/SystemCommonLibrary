using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemCommonLibrary.API.Win
{
	[StructLayout (LayoutKind.Sequential)]
	public struct BlendFunction
	{
		public static byte AC_SRC_OVER = 0;
		public static byte AC_SRC_ALPHA = 1;

		public BlendFunction (byte BlendOp, byte BlendFlags, byte SourceConstantAlpha, byte AlphaFormat)
		{
			this.BlendOp = BlendOp;
			this.BlendFlags = BlendFlags;
			this.SourceConstantAlpha = SourceConstantAlpha;
			this.AlphaFormat = AlphaFormat;
		}

		public byte BlendOp;
		public byte BlendFlags;
		public byte SourceConstantAlpha;
		public byte AlphaFormat;
	}

	public class WINDOWS
	{

	}
}
