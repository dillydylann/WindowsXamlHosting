// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;

namespace Windows.UI.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("d1498437-3bac-4be5-96cb-f6ad1fa6c4f4")]
    internal interface IPrivateCoreWindowFrameworkSynchronizedResize
    {
        bool SynchronizedResize { set; get; }
        IntPtr GetResizeDCompSynchronizationObject();
    }

    /// <summary>
    /// Provides extra methods for <see cref="CoreWindowResizeManager"/>.
    /// </summary>
    public static class CoreWindowResizeManagerExtensions
    {
        public static void SetSynchronizedResize(this CoreWindowResizeManager coreWindowResizeManager, bool value)
        {
            var privateCoreWindowFrameworkSynchronizedResize = (IPrivateCoreWindowFrameworkSynchronizedResize)(object)coreWindowResizeManager;
            privateCoreWindowFrameworkSynchronizedResize.SynchronizedResize = value;
        }

        public static bool GetSynchronizedResize(this CoreWindowResizeManager coreWindowResizeManager)
        {
            var privateCoreWindowFrameworkSynchronizedResize = (IPrivateCoreWindowFrameworkSynchronizedResize)(object)coreWindowResizeManager;
            return privateCoreWindowFrameworkSynchronizedResize.SynchronizedResize;
        }

        public static IntPtr GetResizeDCompSynchronizationObject(this CoreWindowResizeManager coreWindowResizeManager)
        {
            var privateCoreWindowFrameworkSynchronizedResize = (IPrivateCoreWindowFrameworkSynchronizedResize)(object)coreWindowResizeManager;
            return privateCoreWindowFrameworkSynchronizedResize.GetResizeDCompSynchronizationObject();
        }
    }
}
