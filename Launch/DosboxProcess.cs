using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Avalonia.Threading;
using GalaxyLauncher.Interop.Linux;

namespace GalaxyLauncher.Launch;

public interface IDosboxProcess : INotifyPropertyChanged
{
    public bool HasExited { get; }

    public bool CanStart { get; }

    public bool Start(Program program);

    public bool Terminate();
}

public sealed class DosboxProcess : IDosboxProcess, IDisposable
{
    private readonly IDispatcher _dispatcher;
    private readonly string _executablePath;
    private Process? _process;

    public DosboxProcess(IPathFinder pathFinder, IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _executablePath = GetExecutablePath(pathFinder);
        CanStart = File.Exists(_executablePath);
    }

    public void Dispose()
    {
        _process?.Dispose();
        _process = null;
    }

    public bool HasExited => _process?.HasExited ?? true;

    public bool CanStart { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool Start(Program program)
    {
        if (!CanStart || !HasExited) return false;

        _process = Process.Start(_executablePath, ["--conf", program.ConfigPath, "--working-dir", program.Path]);
        if (_process == null) return false;

        OnPropertyChanged(nameof(HasExited));
        _process.EnableRaisingEvents = true;
        _process.Exited += OnExited;

        return true;
    }

    public bool Terminate()
    {
        if (_process == null || _process.HasExited) return false;
        return OperatingSystem.IsLinux()
            ? _process.Kill(PosixSignal.SIGTERM)
            : _process.CloseMainWindow();
    }

    public void Kill()
    {
        if (_process == null || _process.HasExited) return;
        _process.Kill();
    }

    public bool WaitForExit(int milliseconds = -1)
    {
        return _process?.WaitForExit(milliseconds) ?? true;
    }

    private void OnExited(object? sender, EventArgs e)
    {
        _dispatcher.Post(
            () => OnPropertyChanged(nameof(HasExited)),
            DispatcherPriority.Background
        );
    }

    private static string GetExecutablePath(IPathFinder pathFinder)
    {
        var dosboxPath = pathFinder.Find("dosbox");
        if (dosboxPath == null) return "";

        var executableName = OperatingSystem.IsWindows() ? "dosbox.exe" : "dosbox";
        return Path.Combine(dosboxPath, executableName);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}