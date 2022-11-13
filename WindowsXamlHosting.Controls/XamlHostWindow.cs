// Part of the control wrappers of the Windows XAML Hosting API
// Copyright (c) 2021 Dylan Briedis <dylan@dylanbriedis.com>

#pragma warning disable CS4014

using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Drawing = System.Drawing;
using WinForms = System.Windows.Forms;

namespace Windows.UI.Xaml.Hosting.Controls
{
    [global::System.ComponentModel.DesignerCategory("")]
    public class XamlHostWindow : WinForms.Form, ICoreApplicationView
    {
        #region ICoreApplicationView Implementation
        CoreWindow ICoreApplicationView.CoreWindow => coreWindow;

        bool ICoreApplicationView.IsMain => false;
        bool ICoreApplicationView.IsHosted => false;

        event TypedEventHandler<CoreApplicationView, IActivatedEventArgs> ICoreApplicationView.Activated
        {
            add => coreApplicationViewActivated += value;
            remove => coreApplicationViewActivated -= value;
        }
        #endregion

        [field: ThreadStatic]
        public static XamlHostWindow Current { get; private set; }

        private bool initialized = false;

        private CoreWindow coreWindow;
        private CoreWindowEx coreWindowEx;
        private UISettings uiSettings;
        private DisplayInformation displayInformation;

        private XamlHostType xamlHostType;
        private IFrameworkView_Modified frameworkView;
        private XamlPresenter xamlPresenter;

        private TypedEventHandler<CoreApplicationView, IActivatedEventArgs> coreApplicationViewActivated;

        public XamlHostWindow(XamlHostType hostType = XamlHostType.Default, bool useCoreDispatcherAsSynchronizationContext = true)
        {
            xamlHostType = hostType;

            coreWindow = CoreWindowFactory.Create(string.Empty, 0, 0, ClientSize.Width, ClientSize.Height, 0, Handle);
            coreWindowEx = (CoreWindowEx)coreWindow;

            if (Environment.OSVersion.Version >= new Version(10, 0, 17763, 0))
            {
                // Fixes the live visual tree toolbar
                CoreApplicationEx.CreateNonImmersiveView();
            }

            uiSettings = new UISettings();
            displayInformation = DisplayInformation.GetForCurrentView();

            if (useCoreDispatcherAsSynchronizationContext)
            {
                coreWindow.Dispatcher.SetAsCurrentSynchronizationContext();
            }

            if (xamlHostType == XamlHostType.Default)
            {
                if (Environment.OSVersion.Version >= new Version(10, 0, 18362, 0))
                    xamlHostType = XamlHostType.FrameworkView;
                else
                    xamlHostType = XamlHostType.XamlPresenter;
            }

            switch (xamlHostType)
            {
                case XamlHostType.FrameworkView:
                    frameworkView = (IFrameworkView_Modified)(object)new FrameworkView();
                    frameworkView.Initialize(this);
                    frameworkView.SetWindow(coreWindow);
                    Window.Current.SetTransparentBackground(false);
                    break;
                case XamlHostType.XamlPresenter:
                    xamlPresenter = XamlPresenter.CreateFromCoreWindow(coreWindow);
                    xamlPresenter.TransparentBackground = false;
                    break;
            }

            NativeMethods.SetParent(coreWindowEx.WindowHandle, Handle);
            NativeMethods.SetWindowLong(coreWindowEx.WindowHandle, NativeMethods.GWL_STYLE, new IntPtr(NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE));

            coreWindowEx.CoreWindowResizeManager.ShouldWaitForLayoutCompletion = true;
            HostBackdropHelper.EnableHostBackdropBrush(Handle);

            UpdatePresenterTheme();

            if (xamlPresenter != null)
            {
                // Switch the presenter theme when the system theme changes
                uiSettings.ColorValuesChanged += (s, e) => coreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdatePresenterTheme);
            }

            Current = this;
            initialized = true;
        }

        public void ActivateApplication(IActivatedEventArgs args)
        {
            coreApplicationViewActivated?.Invoke(null, args);
        }

        public void RunMessageLoop()
        {
            if (!initialized)
                return;

            switch (xamlHostType)
            {
                case XamlHostType.FrameworkView:
                    frameworkView.Run();
                    break;
                case XamlHostType.XamlPresenter:
                    coreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
                    break;
            }
        }

        public void ShowAndRunMessageLoop()
        {
            Show();
            RunMessageLoop();
        }

        public UIElement Content
        {
            get
            {
                if (!initialized)
                    return null;

                switch (xamlHostType)
                {
                    case XamlHostType.FrameworkView:
                        return Window.Current.Content;
                    case XamlHostType.XamlPresenter:
                        return xamlPresenter.Content;
                    default:
                        return null;
                }
            }
            set
            {
                if (!initialized)
                    return;

                switch (xamlHostType)
                {
                    case XamlHostType.FrameworkView:
                        Window.Current.Content = value;
                        break;
                    case XamlHostType.XamlPresenter:
                        xamlPresenter.Content = value;
                        break;
                }
            }
        }

        public CoreDispatcher Dispatcher => initialized ? coreWindow.Dispatcher : null;

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                NativeMethods.SetWindowText(coreWindowEx.WindowHandle, base.Text);
            }
        }

        #region Theming

        public bool IsUsingSystemTheme { get; set; } = true;

        void SetPresenterTheme(ApplicationTheme theme)
        {
            if (theme == ApplicationTheme.Dark)
            {
                BackColor = Drawing.Color.Black;
                DarkModeAPI.SetWindowDarkMode(Handle, true);

                try
                {
                    if (frameworkView != null)
                        Application.Current.RequestedTheme = ApplicationTheme.Dark;
                    if (xamlPresenter != null)
                        xamlPresenter.RequestedTheme = ApplicationTheme.Dark;
                }
                catch { }
            }
            else
            {
                BackColor = Drawing.Color.White;
                DarkModeAPI.SetWindowDarkMode(Handle, false);

                try
                {
                    if (frameworkView != null)
                        Application.Current.RequestedTheme = ApplicationTheme.Light;
                    if (xamlPresenter != null)
                        xamlPresenter.RequestedTheme = ApplicationTheme.Light;
                }
                catch { }
            }
        }

        void UpdatePresenterTheme()
        {
            if (!IsUsingSystemTheme)
                return;

            if (uiSettings.GetColorValue(UIColorType.Background) == Colors.Black) // Dark theme
            {
                SetPresenterTheme(ApplicationTheme.Dark);
            }
            else
            {
                SetPresenterTheme(ApplicationTheme.Light);
            }
        }

        public ApplicationTheme RequestedTheme
        {
            get
            {
                if (frameworkView != null)
                    return Application.Current.RequestedTheme;
                else if (xamlPresenter != null)
                    return xamlPresenter.RequestedTheme;
                else
                    return 0;
            }
            set
            {
                IsUsingSystemTheme = false;
                SetPresenterTheme(value);
            }
        }

        #endregion

        #region Events

        private void SendVisibilityChangedMessage(bool visible)
        {
            if (coreWindow.Visible != visible)
            {
                NativeMethods.SendMessage(coreWindowEx.WindowHandle, 0x0270, IntPtr.Zero, (IntPtr)(visible ? 1 : 0));
            }
        }

        protected override void OnMove(EventArgs e)
        {
            if (!initialized)
                return;

            NativeMethods.SendMessage(coreWindowEx.WindowHandle, NativeMethods.WM_MOVE, IntPtr.Zero, IntPtr.Zero);
            base.OnMove(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (initialized && WindowState != WinForms.FormWindowState.Minimized)
            {
                Window.Current.MoveWindow(0, 0, (int)(ClientSize.Width / (displayInformation.LogicalDpi / 96)), (int)(ClientSize.Height / (displayInformation.LogicalDpi / 96)));
            }

            SendVisibilityChangedMessage(WindowState != WinForms.FormWindowState.Minimized);
            base.OnResize(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            if (uiSettings != null)
            {
                UpdatePresenterTheme(); // Fallback for when automatic change fails
            }

            base.OnActivated(e);
        }

        protected override void OnFormClosed(WinForms.FormClosedEventArgs e)
        {
            if (!initialized)
                return;

            coreWindow.Close();
            base.OnFormClosed(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!initialized)
                return;

            SendVisibilityChangedMessage(Visible);
            base.OnVisibleChanged(e);
        }

        protected override void WndProc(ref WinForms.Message m)
        {
            base.WndProc(ref m);

            if (!initialized)
                return;

            switch (m.Msg)
            {
                case NativeMethods.WM_ACTIVATE:
                    m.Result = NativeMethods.SendMessage(coreWindowEx.WindowHandle, m.Msg, m.WParam, m.LParam);
                    break;

                case NativeMethods.WM_QUIT:
                    {
                        switch (xamlHostType)
                        {
                            case XamlHostType.FrameworkView:
                                frameworkView.Uninitialize();
                                frameworkView = null;
                                break;
                            case XamlHostType.XamlPresenter:
                                xamlPresenter = null;
                                break;
                        }

                        initialized = false;
                    }
                    break;
            }
        }

        #endregion
    }
}
