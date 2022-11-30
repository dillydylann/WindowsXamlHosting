// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.System;

namespace Windows.UI.Core
{
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("f5f84c8f-cfd0-4cd6-b66b-c5d26ff1689d")]
    internal interface IMessageDispatcher
    {
        void PumpMessages();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("34fff979-bd36-4bb1-82d2-785a605b81fb")]
    internal interface IInternalDispatcher
    {
        void WaitAndProcessMessages(IntPtr hEventWait);
        void TerminateProcessEvents();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("c560466f-67d6-4b40-a1f3-15675e6984ec")]
    internal interface IInternalDispatcher2
    {
        DispatcherQueue DispatcherQueue { get; }
    }

    /// <summary>
    /// Provides extra methods for <see cref="CoreDispatcher"/>.
    /// </summary>
    public static class CoreDispatcherExtensions
    {
        public static void PumpMessages(this CoreDispatcher coreDispatcher)
        {
            var messageDispatcher = (IMessageDispatcher)(object)coreDispatcher;
            messageDispatcher.PumpMessages();
        }


        public static void WaitAndProcessMessages(this CoreDispatcher coreDispatcher, IntPtr hEventWait)
        {
            var internalDispatcher = (IInternalDispatcher)(object)coreDispatcher;
            internalDispatcher.WaitAndProcessMessages(hEventWait);
        }

        public static void TerminateProcessEvents(this CoreDispatcher coreDispatcher)
        {
            var internalDispatcher = (IInternalDispatcher)(object)coreDispatcher;
            internalDispatcher.TerminateProcessEvents();
        }


        public static DispatcherQueue GetDispatcherQueue(this CoreDispatcher coreDispatcher)
        {
            var internalDispatcher2 = (IInternalDispatcher2)(object)coreDispatcher;
            return internalDispatcher2.DispatcherQueue;
        }


        public static void SetAsCurrentSynchronizationContext(this CoreDispatcher coreDispatcher)
        {
            // Async controller for main thread
            object syncContextFactory = Activator.CreateInstance(Assembly.Load("System.Runtime.WindowsRuntime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089").GetType("System.Threading.WinRTSynchronizationContextFactory"));
            SynchronizationContext.SetSynchronizationContext(syncContextFactory.GetType().GetMethod("Create", BindingFlags.Instance | BindingFlags.Public).Invoke(syncContextFactory, new[] { coreDispatcher }) as SynchronizationContext);
        }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [ComImport, Guid("4b4d0861-d718-4f7c-bec7-735c065f7c73")]
    internal interface IInternalCoreDispatcherStatic
    {
        CoreDispatcher GetForCurrentThread();
        CoreDispatcher GetOrCreateForCurrentThread();
    }

    /// <summary>
    /// Provides the factory for <see cref="CoreDispatcher"/>s.
    /// </summary>
    public static class CoreDispatcherFactory
    {
        private static IInternalCoreDispatcherStatic internalCoreDispatcherStatic
            = (IInternalCoreDispatcherStatic)WindowsRuntimeMarshal.GetActivationFactory(typeof(CoreDispatcher));

        public static CoreDispatcher GetForCurrentThread()
        {
            return internalCoreDispatcherStatic.GetForCurrentThread();
        }

        public static CoreDispatcher GetOrCreateForCurrentThread()
        {
            return internalCoreDispatcherStatic.GetOrCreateForCurrentThread();
        }
    }
}
