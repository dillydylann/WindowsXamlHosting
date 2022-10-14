// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.ViewManagement;

namespace Windows.UI.Core
{
    /// <summary>
    /// A wrapper class for extra functionality of <see cref="CoreWindow"/>.
    /// </summary>
    public sealed class CoreWindowEx
    {
        public CoreWindowEx(CoreWindow cw) => coreWindow = cw ?? throw new ArgumentNullException("cw");
        internal CoreWindow coreWindow;

        private IInternalCoreWindow internalCoreWindow => (IInternalCoreWindow)(object)coreWindow;
        private IInternalCoreWindow2 internalCoreWindow2 => (IInternalCoreWindow2)(object)coreWindow;
        private IInternalCoreWindow3 internalCoreWindow3 => (IInternalCoreWindow3)(object)coreWindow;
        private IInternalCoreWindow4 internalCoreWindow4 => (IInternalCoreWindow4)(object)coreWindow;
        private IInternalCoreWindow5 internalCoreWindow5 => (IInternalCoreWindow5)(object)coreWindow;
        private IInternalCoreWindow6 internalCoreWindow6 => (IInternalCoreWindow6)(object)coreWindow;
        private ILastInputEventTimestamp lastInputEventTimestamp => (ILastInputEventTimestamp)(object)coreWindow;
        private IPrivateCoreWindow privateCoreWindow => (IPrivateCoreWindow)(object)coreWindow;
        private ICoordinateConversion coordinateConversion => (ICoordinateConversion)(object)coreWindow;
        private IInternalCoreWindow_Occlusion internalCoreWindow_Occlusion => (IInternalCoreWindow_Occlusion)(object)coreWindow;
        private ICoreWindowInterop coreWindowInterop => (ICoreWindowInterop)(object)coreWindow;
        private ICoreWindowComponentInterop coreWindowComponentInterop => (ICoreWindowComponentInterop)(object)coreWindow;


        // IInternalCoreWindow
        public MouseDevice MouseDevice { get => internalCoreWindow.MouseDevice; }
        public ulong ApplicationViewState { get => internalCoreWindow.ApplicationViewState; }
        public ulong ApplicationViewOrientation { get => internalCoreWindow.ApplicationViewOrientation; }
        public ulong AdjacentDisplayEdges { get => internalCoreWindow.AdjacentDisplayEdges; }
        public bool IsOnLockScreen { get => internalCoreWindow.IsOnLockScreen; }
        public PointerVisualizationSettings PointerVisualizationSettings { get => internalCoreWindow.PointerVisualizationSettings; }
        public CoreWindowResizeManager CoreWindowResizeManager { get => internalCoreWindow.CoreWindowResizeManager; }
        public bool IsScreenCaptureEnabled { get => internalCoreWindow.IsScreenCaptureEnabled; set => internalCoreWindow.IsScreenCaptureEnabled = value; }
        public FullScreenType SuppressSystemOverlays { get => internalCoreWindow.SuppressSystemOverlays; set => internalCoreWindow.SuppressSystemOverlays = value; }
        public void OnMouseDeviceListenerChange() => internalCoreWindow.OnMouseDeviceListenerChange();
        public event TypedEventHandler<CoreWindow, CoreWindowEventArgs> ThemeChanged { add => internalCoreWindow.ThemeChanged += value; remove => internalCoreWindow.ThemeChanged -= value; }
        public event TypedEventHandler<CoreWindow, object> ContextMenuRequested { add => internalCoreWindow.ContextMenuRequested += value; remove => internalCoreWindow.ContextMenuRequested -= value; }
        public event TypedEventHandler<CoreWindow, object> DisplayChanged { add => internalCoreWindow.DisplayChanged += value; remove => internalCoreWindow.DisplayChanged -= value; }
        public event TypedEventHandler<CoreWindow, object> Consolidated { add => internalCoreWindow.Consolidated += value; remove => internalCoreWindow.Consolidated -= value; }

        // IInternalCoreWindow2
        public Rect LayoutBounds { get => internalCoreWindow2.LayoutBounds; }
        public Rect VisibleBounds { get => internalCoreWindow2.VisibleBounds; }
        public ApplicationViewBoundsMode DesiredBoundsMode { get => internalCoreWindow2.DesiredBoundsMode; }
        public bool SetDesiredBoundsMode(ApplicationViewBoundsMode boundsMode) => internalCoreWindow2.SetDesiredBoundsMode(boundsMode);
        public void OnVisibleBoundsChange() => internalCoreWindow2.OnVisibleBoundsChange();
        public event TypedEventHandler<CoreWindow, object> LayoutBoundsChanged { add => internalCoreWindow2.LayoutBoundsChanged += value; remove => internalCoreWindow2.LayoutBoundsChanged -= value; }
        public event TypedEventHandler<CoreWindow, object> VisibleBoundsChanged { add => internalCoreWindow2.VisibleBoundsChanged += value; remove => internalCoreWindow2.VisibleBoundsChanged -= value; }
        public event TypedEventHandler<CoreWindow, KeyEventArgs> SysKeyDown { add => internalCoreWindow2.SysKeyDown += value; remove => internalCoreWindow2.SysKeyDown -= value; }
        public event TypedEventHandler<CoreWindow, KeyEventArgs> SysKeyUp { add => internalCoreWindow2.SysKeyUp += value; remove => internalCoreWindow2.SysKeyUp -= value; }
        public event TypedEventHandler<CoreWindow, object> WindowPositionChanged { add => internalCoreWindow2.WindowPositionChanged += value; remove => internalCoreWindow2.WindowPositionChanged -= value; }
        public event TypedEventHandler<CoreWindow, object> SettingChanged { add => internalCoreWindow2.SettingChanged += value; remove => internalCoreWindow2.SettingChanged -= value; }
        public event TypedEventHandler<CoreWindow, object> ViewStateChanged { add => internalCoreWindow2.ViewStateChanged += value; remove => internalCoreWindow2.ViewStateChanged -= value; }
        public event TypedEventHandler<CoreWindow, CoreWindowEventArgs> Destroying { add => internalCoreWindow2.Destroying += value; remove => internalCoreWindow2.Destroying -= value; }

        // IInternalCoreWindow3
        public void OnNavigateBackRequest() => internalCoreWindow3.OnNavigateBackRequest();
        public void EnableWindow(bool enable) => internalCoreWindow3.EnableWindow(enable);
        public void OnNoPointerActivateChange(bool noPointerActivate) => internalCoreWindow3.OnNoPointerActivateChange(noPointerActivate);
        public void OnInputDelegated(uint sourceViewInstanceId, bool delegated, out ulong delegateWindowHandle) => internalCoreWindow3.OnInputDelegated(sourceViewInstanceId, delegated, out delegateWindowHandle);
        public event TypedEventHandler<CoreWindow, object> InputDelegated { add => internalCoreWindow3.InputDelegated += value; remove => internalCoreWindow3.InputDelegated -= value; }

        // IInternalCoreWindow4
        public void OnResizeStarted() => internalCoreWindow4.OnResizeStarted();
        public void OnResizeCompleted() => internalCoreWindow4.OnResizeCompleted();
        public ComponentDisplayInformation ComponentDisplayInformation { get { var cdi = internalCoreWindow4.ComponentDisplayInformation; return cdi != null ? new ComponentDisplayInformation(cdi) : null; } }
        public event TypedEventHandler<CoreWindow, object> ComponentDisplayInformationChanged { add => internalCoreWindow4.ComponentDisplayInformationChanged += value; remove => internalCoreWindow4.ComponentDisplayInformationChanged -= value; }
        public ComponentDisplayInformation ComponentDisplayInformationPrivate { get { var cdi = internalCoreWindow4.ComponentDisplayInformationPrivate; return cdi != null ? new ComponentDisplayInformation(cdi) : null; } }
        public void ConfigureGetBoundsBehavior(GetBoundsBehavior behavior) => internalCoreWindow4.ConfigureGetBoundsBehavior(behavior);
        public void ConfigureComponentDisplayInformationBehavior(ComponentDisplayInformationBehavior behavior) => internalCoreWindow4.ConfigureComponentDisplayInformationBehavior(behavior);
        public void ConfigureSetFocusBehaviorOnWindowActivated(SetFocusBehavior behavior) => internalCoreWindow4.ConfigureSetFocusBehaviorOnWindowActivated(behavior);
        public event TypedEventHandler<CoreWindow, object> MoveSizeLoopStarted { add => internalCoreWindow4.MoveSizeLoopStarted += value; remove => internalCoreWindow4.MoveSizeLoopStarted -= value; }
        public event TypedEventHandler<CoreWindow, object> MoveSizeLoopCompleted { add => internalCoreWindow4.MoveSizeLoopCompleted += value; remove => internalCoreWindow4.MoveSizeLoopCompleted -= value; }
        public void ConfigureHostRightsForComponent(HostRightFlags hostRightFlagsMask, HostRightFlags hostRightFlags) => internalCoreWindow4.ConfigureHostRightsForComponent(hostRightFlagsMask, hostRightFlags);

        // IInternalCoreWindow5
        public void OnActivationStateChange(CoreWindowActivationMode activationMode, CoreWindowActivationMode descendentActivationMode, uint activatedView) => internalCoreWindow5.OnActivationStateChange(activationMode, descendentActivationMode, activatedView);
        public void RegisterComponent(bool register) => internalCoreWindow5.RegisterComponent(register);
        public CoreWindowActivationMode DescendentActivationMode { get => internalCoreWindow5.DescendentActivationMode; }
        public event TypedEventHandler<CoreWindow, object> ComponentInputConfigured { add => internalCoreWindow5.ComponentInputConfigured += value; remove => internalCoreWindow5.ComponentInputConfigured -= value; }
        public object GetPrivateCoreInput() => internalCoreWindow5.GetPrivateCoreInput();
        public int GetIsComponent() => internalCoreWindow5.GetIsComponent();
        public void ConfigureNonSpatialInputDelegation(uint delegationSourcePID, int delegateInput) => internalCoreWindow5.ConfigureNonSpatialInputDelegation(delegationSourcePID, delegateInput);

        // IInternalCoreWindow6
        public void OnInputDelegatedEx(uint sourceViewInstanceId, uint sourceProcessId, bool delegated, out ulong delegateWindowHandle) => internalCoreWindow6.OnInputDelegatedEx(sourceViewInstanceId, sourceProcessId, delegated, out delegateWindowHandle);
        public void ConfigureInputDelegationByProcess(uint delegationSourcePID, int delegateInput) => internalCoreWindow6.ConfigureInputDelegationByProcess(delegationSourcePID, delegateInput);
        public void ConfigureInputDelegationByView(uint delegationSourceViewID, int delegateInput) => internalCoreWindow6.ConfigureInputDelegationByView(delegationSourceViewID, delegateInput);

        // ILastInputEventTimestamp
        public DateTimeOffset LastInputEventTimestamp { get => lastInputEventTimestamp.LastInputEventTimestamp; }

        // IPrivateCoreWindow
        public bool MayDuplicate { get => privateCoreWindow.MayDuplicate; }
        public void NotifyOnFirstActivation(IntPtr hwndSink) => privateCoreWindow.NotifyOnFirstActivation(hwndSink);
        public bool MainWindow { get => privateCoreWindow.MainWindow; set => privateCoreWindow.MainWindow = value; }
        public bool IsUWP { get => privateCoreWindow.IsUWP; }
        public uint VisibilityDelay { set => privateCoreWindow.VisibilityDelay = value; }
        public void OnPreSuspending() => privateCoreWindow.OnPreSuspending();
        public void OnPostResuming() => privateCoreWindow.OnPostResuming();

        // ICoordinateConversion
        //public CoordinateConversionId CoordinateConversionId { get => coordinateConversion.CoordinateConversionId; }

        // IInternalCoreWindow_Occlusion
        public CoreWindowOcclusion Occlusion { get => internalCoreWindow_Occlusion.Occlusion; }
        public event TypedEventHandler<CoreWindow, object> OcclusionChanged { add => internalCoreWindow_Occlusion.OcclusionChanged += value; remove => internalCoreWindow_Occlusion.OcclusionChanged -= value; }
        public void OnOcclusionChanged(CoreWindowOcclusion occlusion) => internalCoreWindow_Occlusion.OnOcclusionChanged(occlusion);

        // ICoreWindowInterop
        public IntPtr WindowHandle { get => coreWindowInterop.WindowHandle; }
        public bool MessageHandled { set => coreWindowInterop.MessageHandled = value; }

        // ICoreWindowComponentInterop
        public void ConfigureComponentInput(uint hostViewInstanceId, IntPtr hwndHost, object inputSourceVisual) => coreWindowComponentInterop.ConfigureComponentInput(hostViewInstanceId, hwndHost, inputSourceVisual);
        public uint GetViewInstanceId() => coreWindowComponentInterop.GetViewInstanceId();


        public static implicit operator CoreWindow(CoreWindowEx cwex) => cwex.coreWindow;
        public static explicit operator CoreWindowEx(CoreWindow cw) => new CoreWindowEx(cw);
        public override bool Equals(object obj) => obj is CoreWindowEx cw ? coreWindow == cw.coreWindow : obj == coreWindow;
        public override int GetHashCode() => coreWindow.GetHashCode();
    }
}
