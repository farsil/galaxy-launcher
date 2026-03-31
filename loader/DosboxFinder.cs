using System;
using System.IO;
using DosboxLauncher.Interop.Windows;

namespace DosboxLauncher.Loader;

public static class DosboxFinder
{
    private static string ExecutableName => OperatingSystem.IsWindows() ? "dosbox.exe" : "dosbox";

    private static string GetDosboxExecutable(string baseDirectory)
    {
        if (OperatingSystem.IsWindows())
        {
            var shortcutPath = Path.Combine(baseDirectory, "dosbox.lnk");
            if (File.Exists(shortcutPath))
            {
                using var shortcutReader = new ShortcutReader();
                shortcutReader.Load(shortcutPath);
                return Path.Combine(shortcutReader.GetPath(), ExecutableName);
            }
        }

        return Path.Combine(baseDirectory, "dosbox", ExecutableName);
    }

    public static string Find(string baseDirectory)
    {
        var executable = GetDosboxExecutable(baseDirectory);
        if (File.Exists(executable)) return executable;
        throw new FileNotFoundException("Dosbox executable not found");
    }
}