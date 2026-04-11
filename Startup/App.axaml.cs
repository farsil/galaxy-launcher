using System;
using System.IO;
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
    private static readonly string BaseDirectory = GetBaseDirectory();
    private static readonly Dispatcher Dispatcher = Dispatcher.UIThread;
    private static readonly StrongReferenceMessenger Messenger = StrongReferenceMessenger.Default;

    private readonly DosboxRunner _dosboxRunner;
    private readonly DosboxState _dosboxState;
    private readonly ProgramLoader _programLoader;

    public App()
    {
        _dosboxState = new DosboxState();
        _programLoader = new ProgramLoader(BaseDirectory, Messenger, Dispatcher);
        _dosboxRunner = new DosboxRunner(BaseDirectory, _dosboxState, Dispatcher);
    }

    private static string GetBaseDirectory()
    {
        if (OperatingSystem.IsLinux())
        {
            // POSIX requires HOME environment variable to be always set
            var dataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME") ??
                           Path.Combine(Environment.GetEnvironmentVariable("HOME")!, ".local/share");

            var homeDir = Path.Combine(dataHome, "galaxy-launcher");
            if (Directory.Exists(homeDir)) return homeDir;

            var dataDirs = Environment.GetEnvironmentVariable("XDG_DATA_DIRS")?.Split(':') ?? [];
            foreach (var dataDir in dataDirs)
            {
                var systemDir = Path.Combine(dataDir, "galaxy-launcher");
                if (Directory.Exists(systemDir)) return systemDir;
            }
        }

        return Directory.GetCurrentDirectory();
    }

    private MainWindow CreateWindow()
    {
        return new MainWindow
        {
            DataContext = new MainWindowViewModel(Messenger, _dosboxState)
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
        }

        Messenger.Register<ProgramLoaderStartRequestMessage>(this, HandleProgramLoaderStartRequest);
        Messenger.Register<ProgramLoaderStopRequestMessage>(this, HandleProgramLoaderStopRequest);
        Messenger.Register<DosboxStartRequestMessage>(this, HandleDosboxStartRequest);
        Messenger.Register<DosboxStopRequestMessage>(this, HandleDosboxStopRequest);

        base.OnFrameworkInitializationCompleted();
    }

    private void HandleDosboxStartRequest(object recipient, DosboxStartRequestMessage message)
    {
        _dosboxRunner.Start(message.Program);
    }

    private void HandleDosboxStopRequest(object recipient, DosboxStopRequestMessage message)
    {
        _dosboxRunner.Kill();
        _dosboxRunner.WaitForExit();
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

    private void HandleDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _dosboxRunner.Kill();
        Messenger.UnregisterAll(this);
    }
}