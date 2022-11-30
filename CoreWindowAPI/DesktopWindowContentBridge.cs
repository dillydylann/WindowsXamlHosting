// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Collections.Generic;
using CoreWindowAPI;
using Windows.Foundation;
using Windows.UI.Composition;

namespace Windows.UI.Core
{
    public sealed class DesktopWindowContentBridge : IDisposable
    {
        private const string ActivatableClassName = "Windows.UI.Core.DesktopWindowContentBridge";

        #region Instance methods

        public DesktopWindowContentBridge(Compositor compositor, IntPtr hwnd)
            : this(NativeMethods.RoActivateInstance(ActivatableClassName))
        {
            interop.Initialize(compositor, hwnd);
        }

        internal DesktopWindowContentBridge(object obj)
        {
            closable = (IClosable)obj;
            bridge = (IDesktopWindowContentBridge)obj;
            interop = (IDesktopWindowContentBridgeInterop)obj;
        }

        internal IClosable closable;
        internal IDesktopWindowContentBridge bridge;
        internal IDesktopWindowContentBridgeInterop interop;


        public void Dispose() => closable.Close();


        public object Content => bridge.Content;

        public float ScaleFactor
        {
            get => bridge.ScaleFactor;
            set => bridge.ScaleFactor = value;
        }

        public void Connect(object content) => bridge.Connect(content);


        public IntPtr Hwnd => interop.Hwnd;

        public float AppliedScaleFactor => interop.AppliedScaleFactor;


        #region Equality

        public override bool Equals(object obj)
        {
            return obj is DesktopWindowContentBridge bridge &&
                   EqualityComparer<IDesktopWindowContentBridge>.Default.Equals(this.bridge, bridge.bridge);
        }

        public override int GetHashCode()
        {
            return 80496784 + EqualityComparer<IDesktopWindowContentBridge>.Default.GetHashCode(bridge);
        }

        public static bool operator ==(DesktopWindowContentBridge left, DesktopWindowContentBridge right)
        {
            return EqualityComparer<DesktopWindowContentBridge>.Default.Equals(left, right);
        }

        public static bool operator !=(DesktopWindowContentBridge left, DesktopWindowContentBridge right)
        {
            return !(left == right);
        }

        #endregion

        #endregion
    }
}
