// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;

namespace Windows.ApplicationModel.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("6090202d-2843-4ba5-9b0d-fc88eecd9ce5")]
    internal interface ICoreApplicationPrivate2
    {
        void InitializeForAttach();
        CoreWindow WaitForActivate();
        CoreApplicationView CreateNonImmersiveView();
    }

    /// <summary>
    /// Provides extra methods for <see cref="CoreApplication"/>.
    /// </summary>
    public static class CoreApplicationEx
    {
        private static ICoreApplicationPrivate2 coreApplicationPrivate2
            = (ICoreApplicationPrivate2)WindowsRuntimeMarshal.GetActivationFactory(typeof(CoreApplication));

        public static CoreApplicationView CreateNonImmersiveView()
        {
            return coreApplicationPrivate2.CreateNonImmersiveView();
        }
    }
}
