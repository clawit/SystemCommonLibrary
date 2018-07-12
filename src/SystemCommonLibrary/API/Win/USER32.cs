using System;
using System.Runtime.InteropServices;

namespace SystemCommonLibrary.API.Win
{
    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    public class USER32
    {
        private const string USER32_DLL = "USER32.DLL";

        [DllImport(USER32_DLL)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport(USER32_DLL)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport(USER32_DLL)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(USER32_DLL)]
        public static extern IntPtr GetActiveWindow();

    }
}