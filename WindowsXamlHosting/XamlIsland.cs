// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Windows.UI.Xaml.Hosting
{
    public sealed class XamlIsland // : Panel
    {
        private const string ActivatableClassName = "Windows.UI.Xaml.Hosting.XamlIsland";

        #region Instance methods

        internal XamlIsland(IXamlIsland i)
        {
            island = i;
        }
        internal XamlIsland(Panel p)
        {
            Panel = p;
            island = (IXamlIsland)Panel;
        }
        ~XamlIsland()
        {
            Panel = null;
            island = null;
        }

        public Panel Panel { get; private set; }
        internal IXamlIsland island;


        public object CompositionIsland { get => island.CompositionIsland; }

        public UIElement Content { get => island.Content; set => island.Content = value; }

        public object FocusController { get => island.FocusController; }

        public object MaterialProperties { get => island.MaterialProperties; set => island.MaterialProperties = value; }

        public void SetScreenOffsetOverride(Point offset) => island.SetScreenOffsetOverride(offset);

        public void SetFocus() => island.SetFocus();


        public override bool Equals(object obj) => obj is XamlIsland i && island == i.island;

        public override int GetHashCode() => island.GetHashCode();

        #endregion

        #region Static methods

        [ThreadStatic]
        private static Lazy<IXamlIslandStatics> theFactory = new Lazy<IXamlIslandStatics>(() =>
        {
            NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlIslandStatics).GUID, out IActivationFactory factory);
            return (IXamlIslandStatics)factory;
        });

        public static XamlIsland GetIslandFromElement(DependencyObject element)
        {
            IXamlIsland island = theFactory.Value.GetIslandFromElement(element);
            Panel panel = Marshal.GetTypedObjectForIUnknown(Marshal.GetIUnknownForObject(island), typeof(Panel)) as Panel;
            return new XamlIsland(panel);
        }

        #endregion
    }
}
