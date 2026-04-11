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
    private readonly DosboxRunner _dosboxRunner;
    private readonly DosboxState _dosboxState = new();
    private readonly StrongReferenceMessenger _messenger = StrongReferenceMessenger.Default;
    private readonly ProgramLoader _programLoader;

    public App()
    {
        var pathFinder = PathFinder.Create();
        var dispatcher = Dispatcher.UIThread;

        _programLoader = new ProgramLoader(pathFinder, _messenger, dispatcher);
        _dosboxRunner = new DosboxRunner(pathFinder, _dosboxState, dispatcher);
    }

    private MainWindow CreateWindow()
    {
        return new MainWindow
        {
            DataContext = new MainWindowViewModel(_messenger, _dosboxState)
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

        _messenger.Register<ProgramLoaderStartRequestMessage>(this, HandleProgramLoaderStartRequest);
        _messenger.Register<ProgramLoaderStopRequestMessage>(this, HandleProgramLoaderStopRequest);
        _messenger.Register<DosboxStartRequestMessage>(this, HandleDosboxStartRequest);
        _messenger.Register<DosboxStopRequestMessage>(this, HandleDosboxStopRequest);

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
        _messenger.UnregisterAll(this);
    }
}