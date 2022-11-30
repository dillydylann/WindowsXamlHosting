// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("30D5A829-7FA4-4026-83BB-D75BAE4EA99E")]
    internal interface IClosable
    {
        void Close();
    }
}
