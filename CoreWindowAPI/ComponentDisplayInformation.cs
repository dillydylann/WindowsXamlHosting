// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;

namespace Windows.UI.Core
{
    public sealed class ComponentDisplayInformation
    {
        private const string ActivatableClassName = "Windows.UI.Core.ComponentDisplayInformation";

        #region Instance methods

        internal ComponentDisplayInformation(IComponentDisplayInformation i) => info = i;
        private IComponentDisplayInformation info;


        public Matrix4x4 AggregateTransform => info.AggregateTransform;
        public Vector2 AggregateScaleFactor => info.AggregateScaleFactor;
        public Rect Bounds => info.Bounds;
        public Vector2 DpiScaleFactor => info.DpiScaleFactor;
        public float RotationAngle => info.RotationAngle;


        public override bool Equals(object obj) => obj is ComponentDisplayInformation i ? info == i.info : false;
        public override int GetHashCode() => info.GetHashCode();

        #endregion

        #region Static methods

        [ThreadStatic]
        private static Lazy<IComponentDisplayInformationFactory> theFactory = new Lazy<IComponentDisplayInformationFactory>(() =>
        {
            NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IComponentDisplayInformationFactory).GUID, out IActivationFactory factory);
            return (IComponentDisplayInformationFactory)factory;
        });

        public static ComponentDisplayInformation CreateComponentDisplayInformation(Matrix4x4 aggregateTransform, Vector2 aggregateScaleFactor, Vector2 dpiScaleFactor, float rotationAngle, Rect bounds)
        {
            IComponentDisplayInformation cdi = theFactory.Value.CreateComponentDisplayInformation(aggregateTransform, aggregateScaleFactor, dpiScaleFactor, rotationAngle, bounds);
            return cdi != null ? new ComponentDisplayInformation(cdi) : null;
        }

        #endregion
    }
}
