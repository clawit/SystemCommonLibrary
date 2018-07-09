using System;
using System.Runtime.InteropServices;

namespace SystemCommonLibrary.API.Win
{
    public class KERNEL32
    {
        private const string KERNEL32_DLL = "KERNEL32.DLL";
         
        [DllImport(KERNEL32_DLL)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}