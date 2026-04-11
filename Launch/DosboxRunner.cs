using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Threading;

namespace GalaxyLauncher.Launch;

public sealed class DosboxRunner
{
    private readonly IDispatcher _dispatcher;
    private readonly IDosboxState _dosboxState;
    private readonly string _executablePath;
    private Process? _process;

    public DosboxRunner(IPathFinder pathFinder, IDosboxState dosboxState, IDispatcher dispatcher)
    {
        _dosboxState = dosboxState;
        _dispatcher = dispatcher;
        _executablePath = GetExecutablePath(pathFinder);
        _dosboxState.IsRunnable = File.Exists(_executablePath);
    }

    private static string GetExecutablePath(IPathFinder pathFinder)
    {
        var dosboxPath = pathFinder.Find("dosbox");
        if (dosboxPath == null) return "";

        var executableName = OperatingSystem.IsWindows() ? "dosbox.exe" : "dosbox";
        return Path.Combine(dosboxPath, executableName);
    }

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
}