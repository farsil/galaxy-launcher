using System;
using System.IO;
using System.Runtime.Versioning;
using GalaxyLauncher.Interop.Windows;

namespace GalaxyLauncher.Launch;

public interface IPathFinder
{
    string? Find(string path);
}

public static class PathFinder
{
    public static IPathFinder Create()
    {
        if (OperatingSystem.IsWindows()) return new WindowsPathFinder();
        if (OperatingSystem.IsLinux()) return new XdgPathFinder();

        throw new PlatformNotSupportedException();
    }

    [SupportedOSPlatform("windows")]
    private sealed class WindowsPathFinder : IPathFinder
    {
        public string? Find(string path)
        {
            var localDir = GetPath(Directory.GetCurrentDirectory(), path);
            if (Directory.Exists(localDir)) return localDir;

            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var localAppDir = GetPath(Path.Combine(localAppData, "GalaxyLauncher"), path);
            if (Directory.Exists(localAppDir)) return localAppDir;

            return null;
        }

        private static string GetPath(string baseDirectory, string path)
        {
            var shortcutPath = Path.Combine(baseDirectory, $"{path}.lnk");
            if (File.Exists(shortcutPath))
            {
                using var shortcutReader = new ShortcutReader();
                shortcutReader.Load(shortcutPath);
                return shortcutReader.GetPath();
            }

            return Path.Combine(baseDirectory, path);
        }
    }

    [SupportedOSPlatform("linux")]
    private sealed class XdgPathFinder : IPathFinder
    {
        public string? Find(string path)
        {
            var localDir = Path.Combine(Directory.GetCurrentDirectory(), path);
            if (Directory.Exists(localDir)) return localDir;

            // POSIX requires HOME environment variable to be always set
            var dataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME") ??
                           Path.Combine(Environment.GetEnvironmentVariable("HOME")!, ".local", "share");

            var homeDir = Path.Combine(dataHome, "galaxy-launcher", path);
            if (Directory.Exists(homeDir)) return homeDir;

            var dataDirs = Environment.GetEnvironmentVariable("XDG_DATA_DIRS")?.Split(':') ?? [];
            foreach (var dataDir in dataDirs)
            {
                var systemDir = Path.Combine(dataDir, "galaxy-launcher", path);
                if (Directory.Exists(systemDir)) return systemDir;
            }

            return null;
        }
    }
}