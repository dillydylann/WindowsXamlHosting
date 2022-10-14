// Part of the internals of the Windows Runtime CoreWindow API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

using System;

namespace Windows.UI.Core
{
    enum _WINDOW_TYPE
    {
        IMMERSIVE_BODY = 0x0,
        IMMERSIVE_DOCK = 0x1,
        IMMERSIVE_HOSTED = 0x2,
        IMMERSIVE_TEST = 0x3,
        IMMERSIVE_BODY_ACTIVE = 0x4,
        IMMERSIVE_DOCK_ACTIVE = 0x5,
        NOT_IMMERSIVE = 0x6,
    }

    /// <summary>
    /// Provides the factory for <see cref="CoreWindow"/>s.
    /// </summary>
    public static class CoreWindowFactory
    {
        public static CoreWindow Create(string windowTitle, int x, int y, int width, int height, uint attributes = 0, IntPtr ownerWindow = default) 
            => NativeMethods.CreateCoreWindow(_WINDOW_TYPE.NOT_IMMERSIVE, windowTitle, x, y, width, height, attributes, ownerWindow, typeof(ICoreWindow).GUID);
    }

#if false
    public enum CoreWindowType
    {
        UnknownWindow = 0x0,
        PrimaryWindow = 0x1,
        ProxyWindow = 0x2,
        NotImmersiveWindow = 0x3,
    }

    internal enum ZBID
    {
        ZBID_DEFAULT = 0x0,
        ZBID_DESKTOP = 0x1,
        ZBID_UIACCESS = 0x2,
        ZBID_IMMERSIVE_IHM = 0x3,
        ZBID_IMMERSIVE_NOTIFICATION = 0x4,
        ZBID_IMMERSIVE_APPCHROME = 0x5,
        ZBID_IMMERSIVE_MOGO = 0x6,
        ZBID_IMMERSIVE_EDGY = 0x7,
        ZBID_IMMERSIVE_INACTIVEMOBODY = 0x8,
        ZBID_IMMERSIVE_INACTIVEDOCK = 0x9,
        ZBID_IMMERSIVE_ACTIVEMOBODY = 0xa,
        ZBID_IMMERSIVE_ACTIVEDOCK = 0xb,
        ZBID_IMMERSIVE_BACKGROUND = 0xc,
        ZBID_IMMERSIVE_SEARCH = 0xd,
        ZBID_GENUINE_WINDOWS = 0xe,
        ZBID_IMMERSIVE_RESTRICTED = 0xf,
        ZBID_SYSTEM_TOOLS = 0x10,
        ZBID_LOCK = 0x11,
        ZBID_ABOVELOCK_UX = 0x12,
    }

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    delegate int ICoreWindow_RuntimeClassInitialize(IntPtr ptr, CoreWindowType windowType,
        [MarshalAs(UnmanagedType.I1)] bool mayDuplicate, UIntPtr dwExStyle, 
        [MarshalAs(UnmanagedType.LPWStr)] string windowTitle, UIntPtr dwStyle, Rect windowSize, ZBID zbid, UIntPtr dwAttributes, UIntPtr dwTypeFlags);

    public static class CoreWindowFactory
    {
        public static CoreWindow Create(CoreWindowType windowType, bool mayDuplicate, ulong dwExStyle, string windowTitle, ulong dwStyle, Rect windowSize, ulong dwAttributes = 0, ulong dwTypeFlags = 0)
        {
            CoreWindow window = (CoreWindow)Activator.CreateInstance(Type.GetTypeFromCLSID(Guid.Parse("4D3BD4B7-AD22-4DC4-B889-311DAE0D8AEB")));
            IntPtr windowPtr = Marshal.GetIUnknownForObject(window);
            IntPtr windowVPtr = Marshal.ReadIntPtr(windowPtr);

            var runtimeClassInitialize = Marshal.GetDelegateForFunctionPointer<ICoreWindow_RuntimeClassInitialize>(Marshal.ReadIntPtr(windowVPtr, 59 * IntPtr.Size));
            var result = runtimeClassInitialize(windowPtr, windowType, mayDuplicate, (UIntPtr)dwExStyle, windowTitle, (UIntPtr)dwStyle, windowSize, ZBID.ZBID_DEFAULT, (UIntPtr)dwAttributes, (UIntPtr)dwTypeFlags);
            if (result != 0)
                Marshal.ThrowExceptionForHR(result);

            return window;
        }
    }
#endif
}
