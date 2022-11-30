// Part of the control wrappers of the Windows XAML Hosting API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.ComponentModel;
using Windows.UI.Core;
using WindowsXamlHostingControls;
using WinForms = System.Windows.Forms;

namespace Windows.UI.Xaml.Hosting.Controls
{
    [global::System.ComponentModel.DesignerCategory("")]
    public class XamlIslandControl : WinForms.Control
    {
        private bool initialized = false;

        private XamlIsland island;
        private DesktopWindowContentBridge contentBridge;

        public XamlIslandControl()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                // Prevent creation in design mode
                return;
            }

            island = Application.Current.CreateIsland();
            contentBridge = new DesktopWindowContentBridge(Window.Current.Compositor, Handle);
            contentBridge.Connect(island.CompositionIsland);

            initialized = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contentBridge.Dispose();
                Application.Current.RemoveIsland(island);
            }

            base.Dispose(disposing);
        }

        public UIElement Content
        {
            get
            {
                if (!initialized)
                    return null;

                return island.Content;
            }
            set
            {
                if (!initialized)
                    return;

                island.Content = value;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (initialized)
            {
                island.SetFocus();
            }
            base.OnGotFocus(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (initialized)
            {
                NativeMethods.SetWindowPos(contentBridge.Hwnd, 0, 0, 0, Width, Height, 0x0040);
            }
            base.OnSizeChanged(e);
        }
    }
}
