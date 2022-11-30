// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Windows.UI.Xaml.Hosting
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("412b49d7-b8b7-416a-b49b-57f9edbef991")]
    internal interface IXamlIsland
    {
        object CompositionIsland { [return: MarshalAs(UnmanagedType.IInspectable)] get; }
        UIElement Content { get; set; }
        object FocusController { [return: MarshalAs(UnmanagedType.IInspectable)] get; }
        object MaterialProperties { [return: MarshalAs(UnmanagedType.IInspectable)] get; [param: MarshalAs(UnmanagedType.IInspectable)] set; }
        void SetScreenOffsetOverride(Point offsetOnScreen);
        void SetFocus();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("3ead2336-b073-456f-bcaf-82587eb63487")]
    internal interface IXamlIslandStatics
    {
        IXamlIsland GetIslandFromElement(DependencyObject element);
    }
}
