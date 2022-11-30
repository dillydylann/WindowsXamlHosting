// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using Windows.UI.Composition;

namespace Windows.UI.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("55931d08-14c7-50b4-8998-86d6339b9bb5")]
    internal interface IDesktopWindowContentBridge
    {
        object Content { [return: MarshalAs(UnmanagedType.IInspectable)] get; }
        float ScaleFactor { get; set; }
        void Connect([MarshalAs(UnmanagedType.IInspectable)] object content);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("37642806-f421-4fd0-9f82-23ae7c776182")]
    internal interface IDesktopWindowContentBridgeInterop
    {
        void Initialize(Compositor compositor, IntPtr hwnd);
        IntPtr Hwnd { get; }
        float AppliedScaleFactor { get; }
    }
}
