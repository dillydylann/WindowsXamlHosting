using System;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using WinForms = System.Windows.Forms;

namespace WindowsXamlHostingTest
{
    static class HostingTypesTest
    {
        static UIElement CreateTestControl(string text)
            => new ToggleButton { Content = text, HorizontalAlignment = HorizontalAlignment.Center };

        public static void HwndXamlPresenter_Test()
        {
            var window = new WinForms.Form { Text = "HwndXamlPresenter", Left = 100, Top = 100, Width = 500, Height = 500, StartPosition = WinForms.FormStartPosition.Manual, FormBorderStyle = WinForms.FormBorderStyle.None };
            var presenter = XamlPresenter.CreateFromHwnd(window.Handle);
            presenter.Content = CreateTestControl("I'm a button in XamlPresenter from HWND");
            Window.Current.SetTransparentBackground(true);
            WinForms.Application.Run(window);
        }

        public static void CoreWindowXamlPresenter_Test()
        {
            var window = CoreWindowFactory.Create("CoreWindowXamlPresenter", 600, 100, 500, 500);
            CoreApplicationEx.CreateNonImmersiveView();
            var presenter = XamlPresenter.CreateFromCoreWindow(window);
            presenter.Content = CreateTestControl("I'm a button in XamlPresenter from CoreWindow");
            Window.Current.SetTransparentBackground(true);
            window.Activate();
            window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
        }

        public static void FrameworkView_Test()
        {
            var window = CoreWindowFactory.Create("FrameworkView", 100, 600, 500, 500);
            var frameworkView = new FrameworkView();
            frameworkView.Initialize(CoreApplicationEx.CreateNonImmersiveView());
            frameworkView.SetWindow(window);
            Window.Current.Content = CreateTestControl("I'm a button in FrameworkView");
            Window.Current.SetTransparentBackground(true);
            window.Activate();
            frameworkView.Run();
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        public static void DesktopWindowXamlSource_Test()
        {
            var window = new WinForms.Form { Text = "DesktopWindowXamlSource", Left = 600, Top = 600, Width = 500, Height = 500, StartPosition = WinForms.FormStartPosition.Manual, FormBorderStyle = WinForms.FormBorderStyle.None };
            var windowsXamlManager = WindowsXamlManager.InitializeForCurrentThread();
            var desktopWindowXamlSource = new DesktopWindowXamlSource();
            ((IDesktopWindowXamlSourceNative)desktopWindowXamlSource).AttachToWindow(window.Handle);
            SetWindowPos(((IDesktopWindowXamlSourceNative)desktopWindowXamlSource).WindowHandle, 0, 0, 0, 500, 500, 0x0040);
            desktopWindowXamlSource.Content = CreateTestControl("I'm a button in DesktopWindowXamlSource");
            Window.Current.SetTransparentBackground(true);
            WinForms.Application.Run(window);
            windowsXamlManager.Dispose();
        }

        public static void RunThreads()
        {
            Thread[] threads = {
                new Thread(HwndXamlPresenter_Test),
                new Thread(CoreWindowXamlPresenter_Test),
                new Thread(FrameworkView_Test),
                new Thread(DesktopWindowXamlSource_Test),
            };
            foreach (var thread in threads)
            {
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }
    }
}
