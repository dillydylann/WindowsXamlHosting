// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Core;

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

        internal XamlPresenter(IXamlPresenter p)
        {
            presenter = p;
            presenter2 = (IXamlPresenter2)p;
            presenterPrivate = (IXamlPresenterPrivate)p;
        }
        ~XamlPresenter()
        {
            presenter = null;
            presenter2 = null;
            presenterPrivate = null;
        }

        internal IXamlPresenter presenter;
        internal IXamlPresenter2 presenter2;
        internal IXamlPresenterPrivate presenterPrivate;

        /// <summary>
        /// Gets or sets the content to host in the presenter.
        /// </summary>
        public UIElement Content { get => presenter.Content; set => presenter.Content = value; }

        public void SetAtlasSizeHint(int width, int height) => presenter.SetAtlasSizeHint(width, height);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InitializePresenter() => presenter.InitializePresenter();


        public ResourceDictionary Resources { get => presenter2.Resources; set => presenter2.Resources = value; }

        public Rect Bounds => presenter2.Bounds;

        public ApplicationTheme RequestedTheme { get => presenter2.RequestedTheme; set => presenter2.RequestedTheme = value; }

        public bool TransparentBackground { get => presenter2.TransparentBackground; set => presenter2.TransparentBackground = value; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InitializePresenter(ApplicationTheme theme) => presenter2.InitializePresenterWithTheme(theme);

        public void SetCaretWidth(int width) => presenter2.SetCaretWidth(width);

        public void SetViewActivityState(bool active, bool allowDWMSnapshot) => presenterPrivate.SetViewActivityState(active, allowDWMSnapshot);


        public override bool Equals(object obj) => obj is XamlPresenter p ? presenter == p.presenter : false;
        public override int GetHashCode() => presenter.GetHashCode();

        #endregion

        #region Static methods

        static XamlPresenter()
        {
            if (!NativeMethods.IsMouseInPointerEnabled())
                NativeMethods.EnableMouseInPointer(true); // Enables pointer messages
        }


        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void InitializePresenterFunc(IntPtr ptr);

        public static bool AutoInitializePresenter { get; set; } = true;

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
            NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlPresenterStatics).GUID, out IActivationFactory factory);
            IXamlPresenterStatics presenterFactory = (IXamlPresenterStatics)factory;
            IntPtr presenterPtr = presenterFactory.CreateFromHwnd(hwnd.ToInt32());

            if (AutoInitializePresenter)
                return new XamlPresenter(InitializeThePresenter(presenterPtr));
            else
                return new XamlPresenter((IXamlPresenter)Marshal.GetObjectForIUnknown(presenterPtr));
        }

        /// <summary>
        /// Gets the <see cref="XamlPresenter"/> for the current active thread.
        /// </summary>
        public static XamlPresenter Current
        {
            get
            {
                NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlPresenterStatics2).GUID, out IActivationFactory factory);
                IXamlPresenterStatics2 presenterFactory = (IXamlPresenterStatics2)factory;

                return new XamlPresenter(presenterFactory.Current);
            }
        }

        /// <summary>
        /// Creates a <see cref="XamlPresenter"/> from a specified <see cref="CoreWindow"/>.
        /// </summary>
        /// <param name="coreWindow">The core window to host the content.</param>
        /// <returns>The created presenter.</returns>
        public static XamlPresenter CreateFromCoreWindow(CoreWindow coreWindow)
        {
            NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlPresenterStatics3).GUID, out IActivationFactory factory);
            IXamlPresenterStatics3 presenterFactory = (IXamlPresenterStatics3)factory;
            IntPtr presenterPtr = presenterFactory.CreateFromCoreWindow(coreWindow);

            if (AutoInitializePresenter)
                return new XamlPresenter(InitializeThePresenter(presenterPtr));
            else
                return new XamlPresenter((IXamlPresenter)Marshal.GetObjectForIUnknown(presenterPtr));
        }

        #endregion
    }
}
