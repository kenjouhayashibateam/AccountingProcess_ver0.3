using System;
using System.Runtime.InteropServices;

namespace WPF.Win32.Api
{
    public static class User32
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);


        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int value);
    }
}
