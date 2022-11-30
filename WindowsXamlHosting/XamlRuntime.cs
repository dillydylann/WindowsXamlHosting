// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using WindowsXamlHosting;

namespace Windows.UI.Xaml.Hosting
{
    /// <summary>
    /// Provides methods for managing the XAML runtime.
    /// </summary>
    public static class XamlRuntime
    {
        private const string ActivatableClassName = "Windows.UI.Xaml.Hosting.XamlRuntime";

        [ThreadStatic]
        private static Lazy<IXamlRuntimeStatics> statics = new Lazy<IXamlRuntimeStatics>(() =>
            (IXamlRuntimeStatics)NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlRuntimeStatics).GUID));

        public static bool EnableImmersiveColors
        {
            get => statics.Value.EnableImmersiveColors;
            set => statics.Value.EnableImmersiveColors = value;
        }

        public static bool EnableWebView
        {
            get => statics.Value.EnableWebView;
            set => statics.Value.EnableWebView = value;
        }

        public static void SetSite(IXamlRuntimeSite site) => statics.Value.SetSite(site);
    }
}
    