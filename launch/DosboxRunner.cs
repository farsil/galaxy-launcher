using System;
using System.Diagnostics;
using System.IO;
using DosboxLauncher.Interop.Windows;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Launch;

public sealed class DosboxRunner
{
    private readonly string _executablePath;
    private Process? _process;

    public DosboxRunner(string baseDirectory)
    {
        _executablePath = GetDosboxExecutablePath(baseDirectory);
        if (!File.Exists(_executablePath)) throw new FileNotFoundException("Dosbox executable not found");
    }

    private static string ExecutableName => OperatingSystem.IsWindows() ? "dosbox.exe" : "dosbox";

    public void Start(Program program)
    {
        if (_process == null || _process.HasExited)
        {
            _process = Process.Start(_executablePath, ["--conf", program.ConfigPath, "--working-dir", program.Path]);
            _process.Exited += OnProcessExited;
            _process.EnableRaisingEvents = true;
            AppMessenger.Send(new DosboxActiveChangeMessage(true));
        }
    }

    public void Kill()
    {
        if (_process is { HasExited: false })
            _process?.Kill();
    }

    private static void OnProcessExited(object? sender, EventArgs e)
    {
        AppMessenger.Send(new DosboxActiveChangeMessage(false));
    }

    private static string GetDosboxExecutablePath(string baseDirectory)
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
}