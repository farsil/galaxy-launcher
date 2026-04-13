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

    private readonly Dispatcher _dispatcher = Dispatcher.UIThread;
    private readonly DosboxProcess _dosboxProcess;
    private readonly StrongReferenceMessenger _messenger = StrongReferenceMessenger.Default;
    private readonly ProgramLoader _programLoader;

    public App()
    {
        var pathFinder = PathFinder.Create();

        _programLoader = new ProgramLoader(pathFinder, _messenger, _dispatcher);
        _dosboxProcess = new DosboxProcess(pathFinder, _dispatcher);
    }

    private MainWindow CreateWindow()
    {
        return new MainWindow
        {
            DataContext = new MainWindowViewModel(_messenger, _dosboxProcess)
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

        _messenger.Register<ProgramLoaderStartRequestMessage>(this, HandleProgramLoaderStartRequest);
        _messenger.Register<ProgramLoaderStopRequestMessage>(this, HandleProgramLoaderStopRequest);

        base.OnFrameworkInitializationCompleted();
    }

    private void HandleProgramLoaderStartRequest(object recipient, ProgramLoaderStartRequestMessage message)
    {
        _programLoader.Start();
    }

    private void HandleProgramLoaderStopRequest(object recipient, ProgramLoaderStopRequestMessage message)
    {
        _programLoader.RequestStop();
        _programLoader.Join();
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