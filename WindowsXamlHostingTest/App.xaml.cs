using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Hosting.Controls;

namespace WindowsXamlHostingTest
{
    public partial class App
    {
        public App()
        {
            XamlRuntime.EnableWebView = false;

            InitializeComponent();
        }

        [STAThread]
        static void Main(string[] args)
        {
            _ = new App();

            HostingTypesTest.RunThreads();

            var window = new XamlHostWindow { Width = 800, Height = 600, Text = "XAML Hosting Test" };
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));
            window.Content = rootFrame;
            window.ShowAndRunMessageLoop();
        }
    }
}
