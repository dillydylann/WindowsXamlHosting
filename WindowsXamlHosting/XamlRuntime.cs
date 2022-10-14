// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Windows.UI.Xaml.Hosting
{
    /// <summary>
    /// Provides methods for managing the XAML runtime.
    /// </summary>
    public static class XamlRuntime
    {
        private const string ActivatableClassName = "Windows.UI.Xaml.Hosting.XamlRuntime";

        [ThreadStatic]
        private static Lazy<IXamlRuntimeStatics> theFactory = new Lazy<IXamlRuntimeStatics>(() =>
        {
            NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlRuntimeStatics).GUID, out IActivationFactory factory);
            return (IXamlRuntimeStatics)factory;
        });


        public static bool EnableImmersiveColors { get => theFactory.Value.EnableImmersiveColors; set => theFactory.Value.EnableImmersiveColors = value; }
       
        public static bool EnableWebView { get => theFactory.Value.EnableWebView; set => theFactory.Value.EnableWebView = value; }

        public static void SetSite(IXamlRuntimeSite site) => theFactory.Value.SetSite(site);
    }
}
