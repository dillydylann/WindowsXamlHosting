// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Collections.Generic;
using System.Numerics;
using CoreWindowAPI;
using Windows.Foundation;

namespace Windows.UI.Core
{
    public sealed class ComponentDisplayInformation
    {
        private const string ActivatableClassName = "Windows.UI.Core.ComponentDisplayInformation";

        #region Instance methods

        internal ComponentDisplayInformation(object obj)
        {
            info = (IComponentDisplayInformation)obj;
        }

        internal IComponentDisplayInformation info;


        public Matrix4x4 AggregateTransform => info.AggregateTransform;

        public Vector2 AggregateScaleFactor => info.AggregateScaleFactor;

        public Rect Bounds => info.Bounds;

        public Vector2 DpiScaleFactor => info.DpiScaleFactor;

        public float RotationAngle => info.RotationAngle;


        #region Equality

        public override bool Equals(object obj)
        {
            return obj is ComponentDisplayInformation information &&
                   EqualityComparer<IComponentDisplayInformation>.Default.Equals(info, information.info);
        }

        public override int GetHashCode()
        {
            return -674051891 + EqualityComparer<IComponentDisplayInformation>.Default.GetHashCode(info);
        }

        public static bool operator ==(ComponentDisplayInformation left, ComponentDisplayInformation right)
        {
            return EqualityComparer<ComponentDisplayInformation>.Default.Equals(left, right);
        }

        public static bool operator !=(ComponentDisplayInformation left, ComponentDisplayInformation right)
        {
            return !(left == right);
        }

        #endregion

        #endregion

        #region Static methods

        [ThreadStatic]
        private static Lazy<IComponentDisplayInformationFactory> factory = new Lazy<IComponentDisplayInformationFactory>(() =>
            (IComponentDisplayInformationFactory)NativeMethods.RoGetActivationFactory(ActivatableClassName, typeof(IComponentDisplayInformationFactory).GUID));

        public static ComponentDisplayInformation CreateComponentDisplayInformation(Matrix4x4 aggregateTransform, Vector2 aggregateScaleFactor, Vector2 dpiScaleFactor, float rotationAngle, Rect bounds)
        {
            var info = factory.Value.CreateComponentDisplayInformation(aggregateTransform, aggregateScaleFactor, dpiScaleFactor, rotationAngle, bounds);
            return info != null ? new ComponentDisplayInformation(info) : null;
        }

        #endregion
    }
}
