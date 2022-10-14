// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;

static class NativeMethods
{
    [DllImport("combase.dll", PreserveSig = false)]
    internal static extern void RoGetActivationFactory(
        [MarshalAs(UnmanagedType.HString)] string activatableClassId,
        in Guid iid,
        out IActivationFactory factory);

    [DllImport("Windows.UI.dll", EntryPoint = "#1500", PreserveSig = false, CallingConvention = CallingConvention.Cdecl)]
    internal static extern CoreWindow CreateCoreWindow(
        _WINDOW_TYPE WindowType,
        [MarshalAs(UnmanagedType.LPWStr)] string pWindowTitle,
        int X,
        int Y,
        int uWidth,
        int uHeight,
        uint dwAttributes,
        IntPtr hOwnerWindow,
        in Guid riid);
}
