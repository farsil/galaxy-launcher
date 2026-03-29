using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace DosboxLauncher.Interop.Windows;

[SupportedOSPlatform("windows")]
internal sealed class ShortcutReader : IDisposable
{
    private const uint STGM_READ = 0;
    private const uint SLGP_DEFAULT = 0;
    private const int MAX_PATH = 260;
    private const uint S_OK = 0;

    // COM objects do not follow normal inheritance rules, casts are necessary
    private readonly IShellLinkW _link = (IShellLinkW)new ShellLink();

    public void Dispose()
    {
        Marshal.ReleaseComObject(_link);
    }

    public void Load(string filename)
    {
        var result = ((IPersistFile)_link).Load(filename, STGM_READ);
        if (result != S_OK) throw new FileLoadException();
    }

    public string GetPath()
    {
        var sb = new StringBuilder(MAX_PATH);
        var result = _link.GetPath(sb, sb.Capacity, out _, SLGP_DEFAULT);
        if (result != S_OK) throw new InvalidOperationException();
        return sb.ToString();
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct WIN32_FIND_DATAW
    {
        public uint dwFileAttributes;
        public long ftCreationTime;
        public long ftLastAccessTime;
        public long ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    private interface IShellLinkW
    {
        uint GetPath([Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath,
            out WIN32_FIND_DATAW pfd, uint fFlags);

        void GetIDList(out IntPtr ppidl);

        void SetIDList(IntPtr pidl);

        void GetDescription([Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);

        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

        void GetWorkingDirectory([Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);

        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

        void GetArguments([Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);

        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

        void GetHotkey(out short pwHotkey);

        void SetHotkey(short wHotkey);

        void GetShowCmd(out int piShowCmd);

        void SetShowCmd(int iShowCmd);

        void GetIconLocation([Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
            int cchIconPath, out int piIcon);

        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

        void Resolve(IntPtr hwnd, uint fFlags);

        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport]
    [Guid("0000010c-0000-0000-c000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IPersist
    {
        [PreserveSig]
        void GetClassID(out Guid pClassID);
    }

    [ComImport]
    [Guid("0000010b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IPersistFile : IPersist
    {
        new void GetClassID(out Guid pClassID);

        [PreserveSig]
        int IsDirty();

        [PreserveSig]
        uint Load([In] [MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);

        [PreserveSig]
        void Save([In] [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
            [In] [MarshalAs(UnmanagedType.Bool)] bool fRemember);

        [PreserveSig]
        void SaveCompleted([In] [MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

        [PreserveSig]
        void GetCurFile([In] [MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
    }

    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    private class ShellLink
    {
    }
}