// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using MS.Internal.IO.Packaging;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml.Hosting.Controls;
using static WindowsXamlHostingControls.NativeMethods;

namespace Windows.UI.Xaml
{
    /// <summary>
    /// Represents a XAML program with UWP activation support.
    /// </summary>
    public sealed class XamlProgram
    {
        public static XamlProgram Current { get; private set; } = null;

        private CoreDispatcher desktopDispatcher;

        // Desktop-only properties
        public string AppInstanceName { get; }
        public bool SingleInstance { get; }
        public event EventHandler<IActivatedEventArgs> Activated;

        public XamlProgram(string appInstanceName = null, bool singleInstance = true)
        {
            if (Current != null)
                throw new Exception($"Only one {nameof(XamlProgram)} can exist!");
            Current = this;

            if (string.IsNullOrEmpty(appInstanceName))
            {
                try
                {
                    AppInstanceName = $"{AppInfo.Current.PackageFamilyName}!{AppInfo.Current.Id}";
                }
                catch
                {
                    throw new ArgumentNullException(nameof(appInstanceName));
                }
            }
            else
            {
                AppInstanceName = appInstanceName;
            }

            SingleInstance = singleInstance;
        }

        public void Run<TDesktopApp, TUWPApp>()
            where TDesktopApp : Application, new()
            where TUWPApp : Application, new()
        {
            var args = Environment.GetCommandLineArgs();

            // Auto-detect which mode to use
            if (args.Length > 1 && args[1].StartsWith("-ServerName:"))
            {
                RunUWP<TUWPApp>();
            }
            else
            {
                RunDesktop<TDesktopApp>();
            }
        }

        public void RunUWP<TUWPApp>()
            where TUWPApp : Application, new()
        {
            // Start in MTA UWP mode
            var thread = new Thread(() => Application.Start((p) => new TUWPApp())) { Name = "UWP Thread" };
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();
        }

        public void RunDesktop<TDesktopApp>()
            where TDesktopApp : Application, new()
        {
            Thread StartThread()
            {
                // Start in STA desktop mode
                var thread = new Thread(DesktopAppThread<TDesktopApp>) { Name = "Desktop UI Thread" };
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                return thread;
            }

            // Single-instance mode
            if (SingleInstance)
            {
                // Check for existing instances
                using var mutex = new Mutex(false, AppInstanceName);
                if (mutex.WaitOne(0, false))
                {
                    var thread = StartThread();

                    // Listen for UWP activation events
                    new Thread(ActivationPipeServerThread) { Name = "Activation Pipe Server Thread", IsBackground = true }.Start();

                    thread.Join();
                }
                else
                {
                    // An instance is already running
                    using var activationPipeClient = new NamedPipeClientStream(".", AppInstanceName, PipeDirection.Out);
                    activationPipeClient.Connect();

                    // Marshal the activation args and send it to the instance
                    Marshal.ThrowExceptionForHR(CoMarshalInterface(new ManagedIStream(activationPipeClient), typeof(IActivatedEventArgs).GUID,
                        GetActivatedEventArgs(), MSHCTX.MSHCTX_LOCAL, IntPtr.Zero, MSHLFLAGS.MSHLFLAGS_NORMAL));

                    activationPipeClient.WaitForPipeDrain();
                }
            }

            // Multi-instance mode
            else
            {
                StartThread().Join();
            }
        }

        private void DesktopAppThread<TDesktopApp>()
            where TDesktopApp : Application, new()
        {
            new TDesktopApp();
            Activated += (s, e) =>
            {
                _ = desktopDispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    XamlHostWindow.Current?.ActivateApplication(e);
                });
            };

            var hostWindow = XamlHostWindow.Current;
            if (hostWindow != null)
            {
                desktopDispatcher = hostWindow.Dispatcher;
                Activated.Invoke(this, GetActivatedEventArgs());
                hostWindow.RunMessageLoop();
            }
            else if (Window.Current != null)
            {
                desktopDispatcher = Window.Current.Dispatcher;
                Activated.Invoke(this, GetActivatedEventArgs());
                desktopDispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
            }
            else
            {
                // App hasn't created the main window, so just exit...
            }
        }

        private IActivatedEventArgs GetActivatedEventArgs()
        {
            // Are we running in packaged mode?
            bool inPackagedMode = false;
            try
            {
                _ = Package.Current;
                inPackagedMode = true;
            }
            catch { }

            if (inPackagedMode)
            {
                return AppInstance.GetActivatedEventArgs();
            }
            else
            {
                return new NormalLaunchActivatedEventArgs(string.Join(" ", Environment.GetCommandLineArgs().Skip(1)));
            }
        }

        private void ActivationPipeServerThread()
        {
            using var activationPipeServer = new NamedPipeServerStream(AppInstanceName, PipeDirection.In, 1, PipeTransmissionMode.Message);
            while (true)
            {
                activationPipeServer.WaitForConnection();

                // Buffer for a nice clean read
                using var unmarshalBufferStream = new MemoryStream();
                activationPipeServer.CopyTo(unmarshalBufferStream);
                unmarshalBufferStream.Position = 0;

                // Unmarshal the activation args and activate the instance
                var ex = Marshal.GetExceptionForHR(CoUnmarshalInterface(new ManagedIStream(unmarshalBufferStream), Guid.Empty,
                    out var activatedArgsUnmarshaled));
                if (ex?.HResult != 0 && activatedArgsUnmarshaled != null)
                {
                    // NOT THREAD-SAFE!
                    Activated?.Invoke(this, (IActivatedEventArgs)activatedArgsUnmarshaled);
                }

                activationPipeServer.Disconnect();
            }
        }
    }
}
