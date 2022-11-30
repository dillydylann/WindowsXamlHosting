// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using CoreWindowAPI;

namespace Windows.UI.Core
{
    /// <summary>
    /// Provides the factory for <see cref="CoreWindow"/>s.
    /// </summary>
    public static class CoreWindowFactory
    {
        public static CoreWindow Create(string windowTitle, int x, int y, int width, int height, uint attributes = 0, IntPtr ownerWindow = default)
        {
            if (CoreWindow.GetForCurrentThread() != null)
                throw new InvalidOperationException("Only one CoreWindow can be created per thread!");

            var windowType = ownerWindow != IntPtr.Zero ? WINDOW_TYPE.IMMERSIVE_HOSTED : WINDOW_TYPE.NOT_IMMERSIVE;
            return NativeMethods.PrivateCreateCoreWindow(windowType, windowTitle, x, y, (uint)width, (uint)height, attributes, ownerWindow, typeof(ICoreWindow).GUID);
        }
    }
}
