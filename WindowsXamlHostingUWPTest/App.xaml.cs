using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Core;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Hosting.Controls;

namespace WindowsXamlHostingUWPTest
{
    public partial class App : Application
    {
        protected ListView LogListView;

        public App()
        {
            InitializeComponent();

            // App events
            EnteredBackground += App_EnteredBackground;
            LeavingBackground += App_LeavingBackground;
            Suspending += App_Suspending;
            Resuming += App_Resuming;
            UnhandledException += App_UnhandledException;
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            // Window events
            args.Window.Activated += Window_Activated;
            args.Window.Closed += Window_Closed;
            args.Window.SizeChanged += Window_SizeChanged;
            args.Window.VisibilityChanged += Window_VisibilityChanged;

            var buttonBar = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Top };
            {
                var clearLogBtn = new Button { Content = "Clear log" };
                clearLogBtn.Click += ClearLogBtn_Click;
                buttonBar.Children.Add(clearLogBtn);

                var registerBackgroundTaskBtn = new Button { Content = "Register background task" };
                registerBackgroundTaskBtn.Click += RegisterBackgroundTaskBtn_Click;
                buttonBar.Children.Add(registerBackgroundTaskBtn);

                var contentDialogTextBoxTestBtn = new Button { Content = "Content dialog text box test" };
                contentDialogTextBoxTestBtn.Click += ContentDialogTextBoxTestBtn_Click;
                buttonBar.Children.Add(contentDialogTextBoxTestBtn);

                var spawnAppWindowTestBtn = new Button { Content = "Spawn AppWindow" };
                spawnAppWindowTestBtn.Click += SpawnAppWindowTestBtn_Click;
                buttonBar.Children.Add(spawnAppWindowTestBtn);
            }

            args.Window.Content = new Grid()
            {
                Children =
                {
                    buttonBar,
                    (LogListView = new ListView()
                    {
                        Margin = new Thickness(0, 35, 0, 0)
                    }),
                },
            };
            args.Window.Activate();

            LogListView.Items.Add("Window created");
        }

        private void ClearLogBtn_Click(object sender, RoutedEventArgs e)
        {
            LogListView.Items.Clear();
        }

        private void RegisterBackgroundTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = "My Background Trigger";
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            
            var task = builder.Register();
        }

        private void ContentDialogTextBoxTestBtn_Click(object sender, RoutedEventArgs e)
        {
            var tb = new TextBox { PlaceholderText = "Enter text here" };
            tb.KeyDown += (s, e) =>
            {
                LogListView.Items.Add("Content dialog text box key down");
            };
            tb.TextChanging += (s, e) =>
            {
                LogListView.Items.Add("Content dialog text box changing");
            };

            _ = new ContentDialog()
            {
                Title = "Content dialog text box test",
                Content = tb,
                CloseButtonText = "Close",
            }.ShowAsync();
        }

        private async void SpawnAppWindowTestBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appWindow = await AppWindow.TryCreateAsync();
                var button = new Button { Content = "Hello from AppWindow!" };
                button.Click += (s, e) => _ = appWindow.CloseAsync();
                ElementCompositionPreview.SetAppWindowContent(appWindow, button);
                await appWindow.TryShowAsync();
            }
            catch (Exception ex)
            {
                _ = new ContentDialog()
                {
                    Title = "Unable to spawn AppWindow",
                    Content = ex.ToString(),
                    CloseButtonText = "Close",
                }.ShowAsync();
            }
        }

        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            LogListView.Items.Add("App entered background");
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            LogListView.Items.Add("App leaving background");
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            LogListView.Items.Add("App suspending");
        }

        private void App_Resuming(object sender, object e)
        {
            LogListView.Items.Add("App resuming");
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogListView.Items.Add("App unhandled exception: " + e.Message);
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            LogListView.Items.Add("Window activated: " + e.WindowActivationState);
        }

        private void Window_Closed(object sender, CoreWindowEventArgs e)
        {
            LogListView.Items.Add("Window closed");
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            LogListView.Items.Add("Window size changed: " + $"{e.Size.Width}, {e.Size.Height}");
        }

        private void Window_VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            LogListView.Items.Add("Window visibility changed: " + e.Visible);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args is ProtocolActivatedEventArgs protocolArgs)
            {
                LogListView.Items.Add("App activated with protocol: " + protocolArgs.Uri.ToString());
            }
            else if (args is StartupTaskActivatedEventArgs startupTaskArgs)
            {
                LogListView.Items.Add("App activated with startup task: " + startupTaskArgs.TaskId);
            }
            else
            {
                LogListView.Items.Add("App activated with " + args.Kind.ToString());
            }
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            LogListView.Items.Add("Time zone changed!");
        }

        protected override void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            LogListView.Items.Add("App cached file updater activated with " + args.CachedFileUpdaterUI.ToString());
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            LogListView.Items.Add("App file activated with " + args.Files[0].Path);
        }

        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            LogListView.Items.Add("App file open picker activated with " + args.FileOpenPickerUI.AllowedFileTypes[0]);
        }

        protected override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            LogListView.Items.Add("App file save picker activated with " + args.FileSavePickerUI.AllowedFileTypes[0]);
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            LogListView.Items.Add("App search activated with " + args.QueryText);
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            LogListView.Items.Add("App share target activated with " + args.ShareOperation.Data.AvailableFormats[0]);
        }
    }

    public class UWPApp : App
    {
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            LogListView.Items.Add("App launched on UWP: " + args.Arguments);
        }
    }

    public class DesktopApp : App
    {
        private XamlHostWindow window;

        public DesktopApp()
        {
            window = new XamlHostWindow { Width = 800, Height = 800 };
            window.Show();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            LogListView.Items.Add("App launched on desktop: " + args.Arguments);
        }
    }

    internal static class Program
    {
        static void Main() => new XamlProgram().Run<DesktopApp, UWPApp>();
    }
}
