// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Windows.UI.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("491d7888-4df3-41a1-bf52-49378b53f1b1")]
    internal interface IPrivateCursor
    {
        IntPtr HCursor { get; }
    }

    /// <summary>
    /// Provides extra methods for <see cref="CoreCursor"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CoreCursorExtensions
    {
        public static IntPtr GetHCursor(this CoreCursor coreCursor)
        {
            var privateCursor = (IPrivateCursor)(object)coreCursor;
            return privateCursor.HCursor;
        }
    }
}
