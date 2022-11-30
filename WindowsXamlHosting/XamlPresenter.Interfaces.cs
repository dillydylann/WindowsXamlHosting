// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI.Core;

namespace Windows.UI.Xaml.Hosting
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("8438b07a-9ce8-4e22-ab5d-811d84699566")]
    internal interface IXamlPresenter
    {
        UIElement Content { get; set; }
        void SetAtlasSizeHint(int width, int height);
        void InitializePresenter();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("1114f710-6d30-4572-b24e-c81cf25f0fa5")]
    internal interface IXamlPresenter2
    {
        ResourceDictionary Resources { get; set; }
        Rect Bounds { get; }
        ApplicationTheme RequestedTheme { get; set; }
        bool TransparentBackground { get; set; }
        void InitializePresenterWithTheme(ApplicationTheme theme);
        void SetCaretWidth(int width);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("62d7b5f2-2eaf-4ccd-bb91-87ad6e01c92d")]
    internal interface IXamlPresenterPrivate
    {
        void SetViewActivityState(bool active, bool allowDWMSnapshot);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("5c6ef05e-f60d-4433-8bc6-40586456afeb")]
    internal interface IXamlPresenterStatics
    {
        // Has to return as IntPtr due to a bug in ReferenceTrackerManager on versions older than RS5 (1809)
        IntPtr CreateFromHwnd(int hwnd);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("d0c1e6c3-1d35-4770-9c3b-e3ff2eefcc25")]
    internal interface IXamlPresenterStatics2
    {
        IXamlPresenter Current { get; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("a49dea01-9e75-49f0-beee-ef1592fbc82b")]
    internal interface IXamlPresenterStatics3
    {
        // Has to return as IntPtr due to a bug in ReferenceTrackerManager on versions older than RS5 (1809)
        IntPtr CreateFromCoreWindow(CoreWindow coreWindow);
    }
}
