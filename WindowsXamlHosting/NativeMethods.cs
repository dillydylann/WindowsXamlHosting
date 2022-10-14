// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

static class NativeMethods
{
    [DllImport("combase.dll", PreserveSig = false)]
    internal static extern void RoGetActivationFactory(
        [MarshalAs(UnmanagedType.HString)] string activatableClassId,
        in Guid iid,
        out IActivationFactory factory);

    [DllImport("combase.dll", PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.IInspectable)]
    internal static extern object RoActivateInstance(
        [MarshalAs(UnmanagedType.HString)] string activatableClassId);

    [DllImport("user32.dll")]
    internal static extern bool IsMouseInPointerEnabled();

    [DllImport("user32.dll", PreserveSig = false)]
    internal static extern void EnableMouseInPointer(bool enable);
}
