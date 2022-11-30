    // Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Windows.UI.Xaml.Hosting
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("3cbcf1bf-2f76-4e9c-96ab-e84b37972554")]
    public interface IDesktopWindowXamlSourceNative
    {
        void AttachToWindow(IntPtr parentWnd);
        IntPtr WindowHandle { get; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("e3dcd8c7-3057-4692-99c3-7b7720afda31")]
    public interface IDesktopWindowXamlSourceNative2
    {
        void AttachToWindow(IntPtr parentWnd);
        IntPtr WindowHandle { get; }

        bool PreTranslateMessage(IntPtr message);
    }

    /// <summary>
    /// Provides extra methods for <see cref="DesktopWindowXamlSource"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DesktopWindowXamlSourceExtensions
    {
        public static void AttachToWindow(this DesktopWindowXamlSource source, IntPtr parentWnd)
        {
            var desktopWindowXamlSourceNative = (IDesktopWindowXamlSourceNative)source;
            desktopWindowXamlSourceNative.AttachToWindow(parentWnd);
        }

        public static IntPtr GetWindowHandle(this DesktopWindowXamlSource source)
        {
            var desktopWindowXamlSourceNative = (IDesktopWindowXamlSourceNative)source;
            return desktopWindowXamlSourceNative.WindowHandle;
        }

        public static void PreTranslateMessage(this DesktopWindowXamlSource source, IntPtr message)
        {
            var desktopWindowXamlSourceNative2 = (IDesktopWindowXamlSourceNative2)source;
            desktopWindowXamlSourceNative2.PreTranslateMessage(message);
        }
    }
}
