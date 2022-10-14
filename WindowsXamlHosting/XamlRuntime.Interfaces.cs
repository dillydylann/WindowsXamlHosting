// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;

namespace Windows.UI.Xaml.Hosting
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("1c35e215-859e-41a3-922b-303b8699a29d")]
    public interface IXamlRuntimeSite
    {
        // I have no idea what this is used for...
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("c805b0c0-6210-4e4f-b76a-e894e8b1a4ad")]
    internal interface IXamlRuntimeStatics
    {
        bool EnableImmersiveColors { get; set; }
        bool EnableWebView { get; set; }
        void SetSite(IXamlRuntimeSite site);
    }
}
