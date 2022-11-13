// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;

namespace Windows.ApplicationModel.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("638BB2DB-451D-4661-B099-414F34FFB9F1")]
    internal interface ICoreApplicationView
    {
        CoreWindow CoreWindow { get; }
        event TypedEventHandler<CoreApplicationView, IActivatedEventArgs> Activated;
        bool IsMain { get; }
        bool IsHosted { get; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("FAAB5CD0-8924-45AC-AD0F-A08FAE5D0324")]
    internal interface IFrameworkView_Modified
    {
        void Initialize(ICoreApplicationView applicationView);
        void SetWindow(CoreWindow window);
        void Load([MarshalAs(UnmanagedType.HString)] string entryPoint);
        void Run();
        void Uninitialize();
    }
}
