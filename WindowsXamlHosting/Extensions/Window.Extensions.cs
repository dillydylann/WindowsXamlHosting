// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Graphics.DirectX;

namespace Windows.UI.Xaml
{
    // FIXME: Find the GUID for this interface
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    public interface IAtlasRequestCallback
    {
        bool AtlasRequest(uint width, uint height, DirectXPixelFormat pixelFormat);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("06636c29-5a17-458d-8ea2-2422d997a922")]
    internal interface IWindowPrivate
    {
        bool TransparentBackground { get; set; }
        void Show();
        void Hide();
        void MoveWindow(int x, int y, int width, int height);
        void SetAtlasSizeHint(uint width, uint height);
        void ReleaseGraphicsDeviceOnSuspend(bool enable);
        void SetAtlasRequestCallback(IAtlasRequestCallback callback);
        void SetSynchronizationInfo(ulong compSyncObject, ulong commitResizeWindow);
        Rect GetWindowContentBoundsForElement(DependencyObject element);
        Rect GetWindowContentLayoutBoundsForElement(DependencyObject element);
    }

    /// <summary>
    /// Provides extra methods for <see cref="Window"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class WindowPrivateExtensions
    {
        public static bool GetTransparentBackground(this Window window)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            return windowPrivate.TransparentBackground;
        }

        public static void SetTransparentBackground(this Window window, bool value)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.TransparentBackground = value;
        }

        public static void Show(this Window window)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.Show();
        }

        public static void Hide(this Window window)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.Hide();
        }

        public static void MoveWindow(this Window window, int x, int y, int width, int height)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.MoveWindow(x, y, width, height);
        }

        public static void SetAtlasSizeHint(this Window window, uint width, uint height)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.SetAtlasSizeHint(width, height);
        }

        public static void ReleaseGraphicsDeviceOnSuspend(this Window window, bool enable)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.ReleaseGraphicsDeviceOnSuspend(enable);
        }

        public static void SetAtlasRequestCallback(this Window window, IAtlasRequestCallback callback)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.SetAtlasRequestCallback(callback);
        }

        public static void SetSynchronizationInfo(this Window window, ulong compSyncObject, ulong commitResizeWindow)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            windowPrivate.SetSynchronizationInfo(compSyncObject, commitResizeWindow);
        }

        public static Rect GetWindowContentBoundsForElement(this Window window, DependencyObject element)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            return windowPrivate.GetWindowContentBoundsForElement(element);
        }

        public static Rect GetWindowContentLayoutBoundsForElement(this Window window, DependencyObject element)
        {
            var windowPrivate = (IWindowPrivate)(object)window;
            return windowPrivate.GetWindowContentLayoutBoundsForElement(element);
        }
    }
}
