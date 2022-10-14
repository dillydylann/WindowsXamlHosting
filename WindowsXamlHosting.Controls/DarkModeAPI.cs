// Part of the control wrappers of the Windows XAML Hosting API
// Copyright (c) 2021 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;

namespace Windows.UI
{
    // FIXME: Should this be moved into its own library?
    public static class DarkModeAPI
    {
        [DllImport("uxtheme.dll", EntryPoint = "#135")]
        public static extern bool AllowDarkModeForApp(bool allow);


        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern void DwmSetWindowAttribute(IntPtr hwnd, int attr, in bool attrValue, int attrSize);
        
        public static void SetWindowDarkMode(IntPtr hwnd, bool dark)
            => DwmSetWindowAttribute(hwnd, Environment.OSVersion.Version >= new Version(10, 0, 18985) ? 20 : 19, dark, Marshal.SizeOf(dark));
    }
}
