using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using GalaxyLauncher.Launch;
using GalaxyLauncher.Main;

namespace GalaxyLauncher.Startup;

public sealed class App : Application
{
    private const int TerminationTimeoutMs = 3000;

    private readonly Dispatcher _dispatcher;
    private readonly DosboxProcess _dosboxProcess;
    private readonly StrongReferenceMessenger _messenger;
    private readonly IPathFinder _pathFinder;

    public App()
    {
        _messenger = StrongReferenceMessenger.Default;
        _dispatcher = Dispatcher.UIThread;
        _pathFinder = PathFinder.Create();
        _dosboxProcess = new DosboxProcess(_pathFinder, _dispatcher);
    }

    private MainWindow CreateWindow()
    {
        return new MainWindow
        {
            DataContext = new MainWindowViewModel(
                _messenger,
                _dosboxProcess,
                new ProgramLoader(_pathFinder, _messenger, _dispatcher)
            )
        };
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = CreateWindow();
            desktop.Exit += HandleDesktopExit;
            PosixSignalRegistration.Create(PosixSignal.SIGTERM, HandleTerminationSignal);
            PosixSignalRegistration.Create(PosixSignal.SIGINT, HandleTerminationSignal);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void HandleTerminationSignal(PosixSignalContext context)
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        Console.WriteLine($"Received {context.Signal}, requesting shutdown");

        context.Cancel = true;
        _dispatcher.Post(() => desktop.TryShutdown());
    }

    private void HandleDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        if (_dosboxProcess.HasExited) return;

        Console.WriteLine("Stopping DOSBox");

        _dosboxProcess.Terminate();
        if (!_dosboxProcess.WaitForExit(TerminationTimeoutMs))
            _dosboxProcess.Kill();
    }
}