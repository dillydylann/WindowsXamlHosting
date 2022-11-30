// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Hosting;
using WindowsXamlHosting;

namespace Windows.UI.Xaml
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("b3ab45d8-6a4e-4e76-a00d-32d4643a9f1a")]
    internal interface IFrameworkApplicationPrivate
    {
        void StartOnCurrentThread(ApplicationInitializationCallback callback);
        IXamlIsland CreateIsland();
        void RemoveIsland(IXamlIsland island);
        void SetSynchronizationWindow(ulong commitResizeWindow);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCreationParameters
    {
        public int Left, Top, Width, Height;

        [MarshalAs(UnmanagedType.I1)]
        public bool TransparentBackground, IsCoreNavigationClient;
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("c45f3f8c-61e6-4f9a-be88-fe4fe6e64f5f")]
    internal interface IFrameworkApplicationStaticsPrivate
    {
        void StartInCoreWindowHostingMode(WindowCreationParameters windowParams, ApplicationInitializationCallback callback);
        void EnableFailFastOnStowedException();
    }

    /// <summary>
    /// Provides extra methods for <see cref="Application"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ApplicationExtensions
    {
        public static void StartOnCurrentThread(this Application application, ApplicationInitializationCallback callback = null)
        {
            var applicationPrivate = (IFrameworkApplicationPrivate)application;
            applicationPrivate.StartOnCurrentThread(callback);
        }

        public static XamlIsland CreateIsland(this Application application)
        {
            var applicationPrivate = (IFrameworkApplicationPrivate)application;
            var island = applicationPrivate.CreateIsland();
            return island != null ? new XamlIsland(island) : null;
        }

        public static void RemoveIsland(this Application application, XamlIsland island)
        {
            var applicationPrivate = (IFrameworkApplicationPrivate)application;
            applicationPrivate.RemoveIsland(island?.island);
        }

        public static void SetSynchronizationWindow(this Application application, ulong commitResizeWindow)
        {
            var applicationPrivate = (IFrameworkApplicationPrivate)application;
            applicationPrivate.SetSynchronizationWindow(commitResizeWindow);
        }
    }

    /// <summary>
    /// Provides extra methods for <see cref="Application"/>.
    /// </summary>
    public static class ApplicationEx
    {
        private static IFrameworkApplicationStaticsPrivate applicationStaticsPrivate
            = (IFrameworkApplicationStaticsPrivate)WindowsRuntimeMarshal.GetActivationFactory(typeof(Application));

        public static void StartInCoreWindowHostingMode(WindowCreationParameters windowParams, ApplicationInitializationCallback callback)
        {
            // Needed for running in CoreWindowHosting mode
            NativeMethods.CoSetASTATestMode(ASTA_TEST_MODE_FLAGS.ROINITIALIZEASTA_ALLOWED);

            applicationStaticsPrivate.StartInCoreWindowHostingMode(windowParams, callback);
        }

        public static void EnableFailFastOnStowedException()
        {
            applicationStaticsPrivate.EnableFailFastOnStowedException();
        }
    }
}
