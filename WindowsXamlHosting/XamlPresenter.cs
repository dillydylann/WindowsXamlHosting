// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI.Core;
using WindowsXamlHosting;

namespace Windows.UI.Xaml.Hosting
{
    /// <summary>
    /// <para>Presents UWP XAML content in a Win32 window (HWND) or a <see cref="CoreWindow"/>.</para>
    /// <para>Unlike <see cref="DesktopWindowXamlSource"/>, this can only be used once per thread and allows you to use the 
    /// <see cref="CoreWindow"/> and <see cref="Window"/> APIs like you would on an UWP app.</para>
    /// <para>For internal use, this is used for the logon screen, credential and UAC dialogs, and certain dialogs in the Settings app.</para>
    /// </summary>
    public sealed class XamlPresenter
    {
        private const string ActivatableClassName = "Windows.UI.Xaml.Hosting.XamlPresenter";

        #region Instance methods

        internal XamlPresenter(object p)
        {
            presenter = (IXamlPresenter)p;
            presenter2 = (IXamlPresenter2)p;
            presenterPrivate = (IXamlPresenterPrivate)p;
        }

        internal IXamlPresenter presenter;
        internal IXamlPresenter2 presenter2;
        internal IXamlPresenterPrivate presenterPrivate;


        /// <summary>
        /// Gets or sets the content to host in the presenter.
        /// </summary>
        public UIElement Content
        {
            get => presenter.Content;
            set => presenter.Content = value;
        }

        public void SetAtlasSizeHint(int width, int height) => presenter.SetAtlasSizeHint(width, height);


        public ResourceDictionary Resources
        {
            get => presenter2.Resources;
            set => presenter2.Resources = value;
        }

        public Rect Bounds => presenter2.Bounds;

        public ApplicationTheme RequestedTheme
        {
            get => presenter2.RequestedTheme;
            set => presenter2.RequestedTheme = value;
        }

        public bool TransparentBackground
        {
            get => presenter2.TransparentBackground;
            set => presenter2.TransparentBackground = value;
        }

        public void SetCaretWidth(int width) => presenter2.SetCaretWidth(width);


        public void SetViewActivityState(bool active, bool allowDWMSnapshot) => presenterPrivate.SetViewActivityState(active, allowDWMSnapshot);


        #region Equality

        public override bool Equals(object obj)
        {
            return obj is XamlPresenter presenter &&
                   EqualityComparer<IXamlPresenter>.Default.Equals(this.presenter, presenter.presenter);
        }

        public override int GetHashCode()
        {
            return 281200083 + EqualityComparer<IXamlPresenter>.Default.GetHashCode(presenter);
        }

        public static bool operator ==(XamlPresenter left, XamlPresenter right)
        {
            return EqualityComparer<XamlPresenter>.Default.Equals(left, right);
        }

        public static bool operator !=(XamlPresenter left, XamlPresenter right)
        {
            return !(left == right);
        }

        #endregion

        #endregion

        #region Static methods

        static XamlPresenter()
        {
            if (!NativeMethods.IsMouseInPointerEnabled())
                NativeMethods.EnableMouseInPointer(true); // Enables pointer messages
        }


        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void InitializePresenterFunc(IntPtr ptr);

        private static IXamlPresenter InitializeThePresenter(IntPtr p)
        {
            if (Environment.OSVersion.Version < new Version(10, 0, 17763))
            {
                // Avoids throwing up errors for ReferenceTrackerManager on versions older than RS5 (1809), kinda hacky tho
                IntPtr xpVTbl = Marshal.ReadIntPtr(p);
                var initFunc = (InitializePresenterFunc)Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(xpVTbl, IntPtr.Size * 9), typeof(InitializePresenterFunc));
                initFunc(p);

                return (IXamlPresenter)Marshal.GetObjectForIUnknown(p);
            }
            else
            {
                var presenter = (IXamlPresenter)Marshal.GetObjectForIUnknown(p);
                presenter.InitializePresenter();
                return presenter;
            }
        }


        /// <summary>
        /// Creates a <see cref="XamlPresenter"/> from a specified window handle (HWND).
        /// </summary>
        /// <param name="hwnd">The window handle to host the content.</param>
        /// <returns>The created presenter.</returns>
        public static XamlPresenter CreateFromHwnd(IntPtr hwnd)
        {
            var factory = (IXamlPresenterStatics)NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlPresenterStatics).GUID);
            return new XamlPresenter(InitializeThePresenter(factory.CreateFromHwnd(hwnd.ToInt32())));
        }

        /// <summary>
        /// Gets the <see cref="XamlPresenter"/> for the current active thread.
        /// </summary>
        public static XamlPresenter Current
        {
            get
            {
                var factory = (IXamlPresenterStatics2)NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlPresenterStatics2).GUID);
                return new XamlPresenter(factory.Current);
            }
        }

        /// <summary>
        /// Creates a <see cref="XamlPresenter"/> from a specified <see cref="CoreWindow"/>.
        /// </summary>
        /// <param name="coreWindow">The core window to host the content.</param>
        /// <returns>The created presenter.</returns>
        public static XamlPresenter CreateFromCoreWindow(CoreWindow coreWindow)
        {
            var factory = (IXamlPresenterStatics3)NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlPresenterStatics3).GUID);
            return new XamlPresenter(InitializeThePresenter(factory.CreateFromCoreWindow(coreWindow)));
        }

        #endregion
    }
}
