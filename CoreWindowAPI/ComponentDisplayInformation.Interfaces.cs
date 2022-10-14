// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Windows.Foundation;

namespace Windows.UI.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("6a4e98ca-1742-468f-8617-1cb0fe5556b9")]
    internal interface IComponentDisplayInformation
    {
        Matrix4x4 AggregateTransform { get; }
        Vector2 AggregateScaleFactor { get; }
        Rect Bounds { get; }
        Vector2 DpiScaleFactor { get; }
        float RotationAngle { get; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("aa67cdd6-573d-4c26-a6f3-071078053537")]
    internal interface IComponentDisplayInformationFactory
    {
        IComponentDisplayInformation CreateComponentDisplayInformation(
            Matrix4x4 aggregateTransform,
            Vector2 aggregateScaleFactor, 
            Vector2 dpiScaleFactor,
            float rotationAngle,
            Rect bounds);
    }
}
