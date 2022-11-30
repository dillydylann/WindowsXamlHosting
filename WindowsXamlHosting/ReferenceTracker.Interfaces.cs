// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Windows.UI.Xaml.Hosting.ReferenceTracker
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("64bd43f8-bfee-4ec4-b7eb-2935158dae21")]
    public interface IReferenceTrackerTarget
    {
        [PreserveSig] uint AddRefFromReferenceTracker();
        [PreserveSig] uint ReleaseFromReferenceTracker();

        void Peg();
        void Unpeg();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("11d3b13a-180e-4789-a8be-7712882893e6")]
    public interface IReferenceTracker
    {
        void ConnectFromTrackerSource();
        void DisconnectFromTrackerSource();
        void FindTrackerTargets(IFindReferenceTargetsCallback callback);

        IReferenceTrackerManager GetReferenceTrackerManager();
        void AddRefFromTrackerSource();
        void ReleaseFromTrackerSource();
        void PegFromTrackerSource();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("3cf184b4-7ccb-4dda-8455-7e6ce99a3298")]
    public interface IReferenceTrackerManager
    {
        void ReferenceTrackingStarted();
        void FindTrackerTargetsCompleted(bool findFailed);
        void ReferenceTrackingCompleted();
        void SetReferenceTrackerHost(IReferenceTrackerHost value);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("04b3486c-4687-4229-8d14-505ab584dd88")]
    public interface IFindReferenceTargetsCallback
    {
        void FoundTrackerTarget(IReferenceTrackerTarget target);
    }

    public enum ReferenceTrackerDisconnectOptions
    {
        Default = 0,
        Suspend = 1
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("29a71c6a-3c42-4416-a39d-e2825a07a773")]
    public interface IReferenceTrackerHost
    {
        void DisconnectUnusedReferenceSources(ReferenceTrackerDisconnectOptions options);
        void ReleaseDisconnectedReferenceSources();
        void NotifyEndOfReferenceTrackingOnThread();

        IReferenceTrackerTarget GetTrackerTarget([MarshalAs(UnmanagedType.IUnknown)] object unknown);

        void AddMemoryPressure(ulong bytesAllocated);
        void RemoveMemoryPressure(ulong bytesAllocated);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("4e897caa-59d5-4613-8f8c-f7ebd1f399b0")]
    public interface IReferenceTrackerExtension
    {
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TrackerHandle : IEquatable<TrackerHandle>
    {
        public IntPtr Value;

        #region Equality

        public override bool Equals(object obj)
        {
            return obj is TrackerHandle handle && Equals(handle);
        }

        public bool Equals(TrackerHandle other)
        {
            return EqualityComparer<IntPtr>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();

        public static bool operator ==(TrackerHandle left, TrackerHandle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TrackerHandle left, TrackerHandle right)
        {
            return !(left == right);
        }

        #endregion
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("eb24c20b-9816-4ac7-8cff-36f67a118f4e")]
    public interface ITrackerOwner
    {
        TrackerHandle CreateTrackerHandle();
        void DeleteTrackerHandle(TrackerHandle handle);
        void SetTrackerValue(TrackerHandle handle, [MarshalAs(UnmanagedType.IUnknown)] object value);
        [PreserveSig] bool TryGetSafeTrackerValue(TrackerHandle handle, [MarshalAs(UnmanagedType.IUnknown)] out object returnValue);
    }
}
