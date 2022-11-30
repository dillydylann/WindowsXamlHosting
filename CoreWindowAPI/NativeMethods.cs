// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;

namespace CoreWindowAPI
{
    internal enum WINDOW_TYPE
    {
        IMMERSIVE_BODY = 0x0,
        IMMERSIVE_DOCK = 0x1,
        IMMERSIVE_HOSTED = 0x2,
        IMMERSIVE_TEST = 0x3,
        IMMERSIVE_BODY_ACTIVE = 0x4,
        IMMERSIVE_DOCK_ACTIVE = 0x5,
        NOT_IMMERSIVE = 0x6,
    }

    internal static class NativeMethods
    {
        [DllImport("combase.dll", PreserveSig = false)]
        internal static extern IActivationFactory RoGetActivationFactory(
            [MarshalAs(UnmanagedType.HString)] string activatableClassId,
            in Guid iid);

        [DllImport("combase.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.IInspectable)]
        internal static extern object RoActivateInstance(
            [MarshalAs(UnmanagedType.HString)] string activatableClassId);

        [DllImport("Windows.UI.dll", EntryPoint = "#1500", PreserveSig = false, CallingConvention = CallingConvention.Cdecl)]
        internal static extern CoreWindow PrivateCreateCoreWindow(
            WINDOW_TYPE WindowType,
            [MarshalAs(UnmanagedType.LPWStr)] string pWindowTitle,
            int X,
            int Y,
            uint uWidth,
            uint uHeight,
            uint dwAttributes,
            IntPtr hOwnerWindow,
            in Guid riid);
    }
}
