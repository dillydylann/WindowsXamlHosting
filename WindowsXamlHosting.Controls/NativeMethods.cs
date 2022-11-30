// Part of the control wrappers of the Windows XAML Hosting API
// Copyright (c) 2021 Dylan Briedis <dylan@dylanbriedis.com>

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WindowsXamlHostingControls
{
    internal static class NativeMethods
    {
    public const int
        GWL_STYLE = -16;

    public const int
        WS_BORDER = 0x800000,
        WS_CAPTION = 0xc00000,
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_DISABLED = 0x8000000,
        WS_DLGFRAME = 0x400000,
        WS_GROUP = 0x20000,
        WS_HSCROLL = 0x100000,
        WS_MAXIMIZE = 0x1000000,
        WS_MAXIMIZEBOX = 0x10000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x20000,
        WS_OVERLAPPED = 0x0,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = unchecked((int)0x80000000),
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_THICKFRAME = 0x40000,
        WS_SYSMENU = 0x80000,
        WS_TABSTOP = 0x10000,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x200000;

    public const int
        WM_MOVE = 0x0003,
        WM_ACTIVATE = 0x0006,
        WM_QUIT = 0x0012;


    public static int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size == 4)
            return SetWindowLong32(hWnd, nIndex, dwNewLong);
        else
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    public static extern int SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowText(IntPtr hwnd, string lpString);

    [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern void PostQuitMessage(int nExitCode);

    public enum MSHCTX : uint
    {
        MSHCTX_LOCAL = 0,
        MSHCTX_NOSHAREDMEM = 1,
        MSHCTX_DIFFERENTMACHINE = 2,
        MSHCTX_INPROC = 3,
        MSHCTX_CROSSCTX = 4
    };

    [Flags]
    public enum MSHLFLAGS : uint
    {
        MSHLFLAGS_NORMAL = 0,
        MSHLFLAGS_TABLESTRONG = 1,
        MSHLFLAGS_TABLEWEAK = 2,
        MSHLFLAGS_NOPING = 4,
        MSHLFLAGS_RESERVED1 = 8,
        MSHLFLAGS_RESERVED2 = 16,
        MSHLFLAGS_RESERVED3 = 32,
        MSHLFLAGS_RESERVED4 = 64
    };

    [DllImport("ole32.dll")]
    public static extern int CoMarshalInterface(IStream pStm, in Guid riid,
        [MarshalAs(UnmanagedType.IUnknown)] object pUnk, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags);

    [DllImport("ole32.dll")]
    public static extern int CoUnmarshalInterface(IStream pStm, in Guid riid,
        [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
}
}
