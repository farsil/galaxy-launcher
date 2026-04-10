using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Threading;
using GalaxyLauncher.Interop.Windows;

namespace GalaxyLauncher.Launch;

public sealed class DosboxRunner
{
    private readonly IDispatcher _dispatcher;
    private readonly IDosboxState _dosboxState;
    private readonly string _executablePath;
    private Process? _process;

    public DosboxRunner(string baseDirectory, IDosboxState dosboxState, IDispatcher dispatcher)
    {
        _dosboxState = dosboxState;
        _dispatcher = dispatcher;
        _executablePath = GetDosboxExecutablePath(baseDirectory);
        _dosboxState.IsRunnable = File.Exists(_executablePath);
    }

    private static string ExecutableName => OperatingSystem.IsWindows() ? "dosbox.exe" : "dosbox";

    public void Start(Program program)
    {
        if (_dosboxState.IsRunnable && (_process == null || _process.HasExited))
        {
            _process = Process.Start(_executablePath, ["--conf", program.ConfigPath, "--working-dir", program.Path]);
            // Process will be eventually garbage collected, eager event handler cleanup is unnecessary
            _process.Exited += HandleProcessExited;
            _process.EnableRaisingEvents = true;
            _dosboxState.IsActive = true;
        }
    }

    public void Kill()
    {
        if (_process is { HasExited: false })
            _process.Kill();
    }

    public void WaitForExit()
    {
        _process?.WaitForExit();
    }

    private void HandleProcessExited(object? sender, EventArgs e)
    {
        _dispatcher.Post(() => _dosboxState.IsActive = false);
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