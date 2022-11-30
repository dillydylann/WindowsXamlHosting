// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WindowsXamlHosting
{
    [Flags]
    internal enum ASTA_TEST_MODE_FLAGS
    {
        NONE = 0x0,
        RO_INIT_SINGLETHREADED_CREATES_ASTAS = 0x1,
        GIT_LIFETIME_EXTENSION_ENABLED = 0x2,
        ROINITIALIZEASTA_ALLOWED = 0x4,
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

        [DllImport("user32.dll")]
        internal static extern bool IsMouseInPointerEnabled();

        [DllImport("user32.dll", PreserveSig = false)]
        internal static extern void EnableMouseInPointer(bool enable);

        [DllImport("combase.dll", EntryPoint = "#100")]
        internal static extern void CoSetASTATestMode(ASTA_TEST_MODE_FLAGS flags);
    }
}
