// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using WindowsXamlHosting;

namespace Windows.UI.Xaml.Hosting
{
    public sealed class XamlIsland // : Panel
    {
        private const string ActivatableClassName = "Windows.UI.Xaml.Hosting.XamlIsland";

        #region Instance methods

        public XamlIsland()
            : this(NativeMethods.RoActivateInstance(ActivatableClassName)) { }

        internal XamlIsland(object obj)
        {
            island = (IXamlIsland)obj;
        }

        internal IXamlIsland island;

        public Panel Panel { get; private set; }


        public object CompositionIsland => island.CompositionIsland;

        public UIElement Content
        {
            get => island.Content;
            set => island.Content = value;
        }

        public object FocusController => island.FocusController;

        public object MaterialProperties
        {
            get => island.MaterialProperties;
            set => island.MaterialProperties = value;
        }

        public void SetScreenOffsetOverride(Point offset) => island.SetScreenOffsetOverride(offset);

        public void SetFocus() => island.SetFocus();


        #region Equality

        public override bool Equals(object obj)
        {
            return obj is XamlIsland island &&
                   EqualityComparer<IXamlIsland>.Default.Equals(this.island, island.island);
        }

        public override int GetHashCode()
        {
            return -705285172 + EqualityComparer<IXamlIsland>.Default.GetHashCode(island);
        }

        public static bool operator ==(XamlIsland left, XamlIsland right)
        {
            return EqualityComparer<XamlIsland>.Default.Equals(left, right);
        }

        public static bool operator !=(XamlIsland left, XamlIsland right)
        {
            return !(left == right);
        }

        #endregion

        #endregion

        #region Static methods

        [ThreadStatic]
        private static Lazy<IXamlIslandStatics> statics = new Lazy<IXamlIslandStatics>(() =>
            (IXamlIslandStatics)NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IXamlIslandStatics).GUID));

        public static XamlIsland GetIslandFromElement(DependencyObject element)
        {
            var island = statics.Value.GetIslandFromElement(element);
            return island != null ? new XamlIsland(island) : null;
        }

        #endregion
    }
}
