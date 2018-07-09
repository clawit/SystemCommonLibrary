using System;
using System.Runtime.InteropServices;

namespace SystemCommonLibrary.API.Win
{
    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Helper class containing Gdi32 API functions
    /// </summary>
    public class USER32
    {
        private const string USER32_DLL = "USER32.DLL";
         

        [DllImport(USER32_DLL)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport(USER32_DLL)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport(USER32_DLL)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }
}