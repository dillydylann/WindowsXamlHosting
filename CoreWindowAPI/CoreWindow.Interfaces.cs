// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.ViewManagement;

namespace Windows.UI.Core
{
    public enum FullScreenType
    {
        Standard = 0x0,
        Minimal = 0x1,
        SuppressSystemOverlays = 0x2,
        None = 0x3,
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("42a17e3d-7171-439a-b1fa-a31b7b957489")]
    internal interface IInternalCoreWindow
    {
        MouseDevice MouseDevice { get; }
        ulong ApplicationViewState { get; }
        ulong ApplicationViewOrientation { get; }
        ulong AdjacentDisplayEdges { get; }
        bool IsOnLockScreen { get; }
        PointerVisualizationSettings PointerVisualizationSettings { get; }
        CoreWindowResizeManager CoreWindowResizeManager { get; }
        bool IsScreenCaptureEnabled { get; set; }
        FullScreenType SuppressSystemOverlays { get; set; }
        void OnMouseDeviceListenerChange();
        event TypedEventHandler<CoreWindow, CoreWindowEventArgs> ThemeChanged;
        event TypedEventHandler<CoreWindow, object> ContextMenuRequested; // ContextMenuRequestedEventArgs
        event TypedEventHandler<CoreWindow, object> DisplayChanged; // DisplayChangedEventArgs
        event TypedEventHandler<CoreWindow, object> Consolidated; // ApplicationViewConsolidatedEventArgsInternal
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("c12779d8-85d2-43e5-901a-95dd4f8ecba3")]
    internal interface IInternalCoreWindow2
    {
        Rect LayoutBounds { get; }
        Rect VisibleBounds { get; }
        ApplicationViewBoundsMode DesiredBoundsMode { get; }
        bool SetDesiredBoundsMode(ApplicationViewBoundsMode boundsMode);
        void OnVisibleBoundsChange();
        event TypedEventHandler<CoreWindow, object> LayoutBoundsChanged;
        event TypedEventHandler<CoreWindow, object> VisibleBoundsChanged;
        event TypedEventHandler<CoreWindow, KeyEventArgs> SysKeyDown;
        event TypedEventHandler<CoreWindow, KeyEventArgs> SysKeyUp;
        event TypedEventHandler<CoreWindow, object> WindowPositionChanged;
        event TypedEventHandler<CoreWindow, object> SettingChanged; // SettingChangedEventArgs
        event TypedEventHandler<CoreWindow, object> ViewStateChanged; // ViewStateChangedEventArgs
        event TypedEventHandler<CoreWindow, CoreWindowEventArgs> Destroying;
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("b8c7c64f-db4f-4ad8-9470-92c21990eaf2")]
    internal interface IInternalCoreWindow3
    {
        void OnNavigateBackRequest();
        void EnableWindow(bool enable);
        void OnNoPointerActivateChange(bool noPointerActivate);
        void OnInputDelegated(uint sourceViewInstanceId, bool delegated, out ulong delegateWindowHandle);
        event TypedEventHandler<CoreWindow, object> InputDelegated; // InputDelegatedEventArgs
    }

    public enum GetBoundsBehavior
    {
        Default = 0x0,
        Legacy = 0x0,
        Logical = 0x1,
        Physical = 0x2,
    }

    public enum ComponentDisplayInformationBehavior
    {
        InvertScalingTransform = 0x0,
        Default = 0x1,
        DontInvertScalingTranform = 0x1,
    }

    public enum SetFocusBehavior
    {
        SetFocusOnWindowActivated = 0x0,
        Default = 0x0,
        DoNotSetFocusOnWindowActivated = 0x1,
    }

    public enum HostRightFlags
    {
        Default = 0x0,
        None = 0x0,
        TakeFocus = 0x1,
        AccessClipboard = 0x2,
        All = 0x3,
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("205ee45c-ad7c-425f-9c19-0f2c0861be77")]
    internal interface IInternalCoreWindow4
    {
        void OnResizeStarted();
        void OnResizeCompleted();
        IComponentDisplayInformation ComponentDisplayInformation { get; }
        event TypedEventHandler<CoreWindow, object> ComponentDisplayInformationChanged; // ComponentDisplayInformationChangedEventArgs
        IComponentDisplayInformation ComponentDisplayInformationPrivate { get; }
        void ConfigureGetBoundsBehavior(GetBoundsBehavior behavior);
        void ConfigureComponentDisplayInformationBehavior(ComponentDisplayInformationBehavior behavior);
        void ConfigureSetFocusBehaviorOnWindowActivated(SetFocusBehavior behavior);
        event TypedEventHandler<CoreWindow, object> MoveSizeLoopStarted;
        event TypedEventHandler<CoreWindow, object> MoveSizeLoopCompleted;
        void ConfigureHostRightsForComponent(HostRightFlags hostRightFlagsMask, HostRightFlags hostRightFlags);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("d6959d35-300c-4b32-a2a7-193be6508e14")]
    internal interface IInternalCoreWindow5
    {
        void OnActivationStateChange(CoreWindowActivationMode activationMode, CoreWindowActivationMode descendentActivationMode, uint activatedView);
        void RegisterComponent(bool register);
        CoreWindowActivationMode DescendentActivationMode { get; }
        event TypedEventHandler<CoreWindow, object> ComponentInputConfigured;
        [return: MarshalAs(UnmanagedType.IInspectable)] object GetPrivateCoreInput();
        int GetIsComponent();
        void ConfigureNonSpatialInputDelegation(uint delegationSourcePID, int delegateInput);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("b8b3facb-498c-40ab-8cf2-b9d6f3101626")]
    internal interface IInternalCoreWindow6
    {
        void OnInputDelegatedEx(uint sourceViewInstanceId, uint sourceProcessId, bool delegated, out ulong delegateWindowHandle);
        void ConfigureInputDelegationByProcess(uint delegationSourcePID, int delegateInput);
        void ConfigureInputDelegationByView(uint delegationSourceViewID, int delegateInput);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("60db7465-c9ec-4b79-ac73-7e0972619b28")]
    public interface ILastInputEventTimestamp
    {
        DateTimeOffset LastInputEventTimestamp { get; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("cd98a184-5273-44ac-bf52-d62342a94bd5")]
    internal interface IPrivateCoreWindow
    {
        bool MayDuplicate { get; }
        void NotifyOnFirstActivation(IntPtr hwndSink);
        bool MainWindow { get; set; }
        bool IsUWP { get; }
        uint VisibilityDelay { set; }
        void OnPreSuspending();
        void OnPostResuming();
    }

    public enum FocusDirection
    {
        None = 0x0,
        Next = 0x1,
        Previous = 0x2,
        Up = 0x3,
        Down = 0x4,
        Left = 0x5,
        Right = 0x6,
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("5f882a05-d594-48bc-9a5a-fd3b9ce90390")]
    public interface IComponentFocus
    {
        void DepartFocus(FocusDirection direction);
        event TypedEventHandler<object, object> FocusReceived; // FocusEventArgs
        void RequestActivation();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CoordinateConversionId
    {
        public long Value;
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("cdf1c1be-efbf-459e-8ad3-7352e9a98807")]
    public interface ICoordinateConversion
    {
        CoordinateConversionId CoordinateConversionId { get; }
    }

    public enum CoreWindowOcclusion
    {
        None = 0x0,
        Full = 0x1,
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("6acfb09d-cb79-4d8b-b1aa-cf07323c3dda")]
    internal interface IInternalCoreWindow_Occlusion
    {
        CoreWindowOcclusion Occlusion { get; }
        event TypedEventHandler<CoreWindow, object> OcclusionChanged; // WindowOcclusionChangedEventArgs
        void OnOcclusionChanged(CoreWindowOcclusion occlusion);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F")]
    public interface ICoreWindowInterop
    {
        IntPtr WindowHandle { get; }
        bool MessageHandled { set; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("40BFE3E3-B75A-4479-AC96-475365749BB8")]
    public interface ICoreInputInterop
    {
        void SetInputSource([MarshalAs(UnmanagedType.IUnknown)] object value);
        bool MessageHandled { set; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0576AB31-A310-4C40-BA31-FD37E0298DFA")]
    public interface ICoreWindowComponentInterop
    {
        void ConfigureComponentInput(uint hostViewInstanceId, IntPtr hwndHost, [MarshalAs(UnmanagedType.IUnknown)] object inputSourceVisual);
        uint GetViewInstanceId();
    }
}
