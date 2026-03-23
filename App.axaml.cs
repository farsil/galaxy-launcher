using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DosboxLauncher.Loader;
using DosboxLauncher.Main;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Startup;

public class App : Application
{
    private readonly ProgramLoader _programLoader = new(".");

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            Messenger.Register<MainWindowStateChangeMessage>(this, OnMainWindowStateChangeMessageReceived);
            desktop.Exit += OnDesktopExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Messenger.UnregisterAll(this);
    }

    private void OnMainWindowStateChangeMessageReceived(object recipient, MainWindowStateChangeMessage message)
    {
        switch (message.Value)
        {
            case MainWindowState.Opened:
                _programLoader.Start();
                break;
            case MainWindowState.Closed:
                _programLoader.RequestStop();
                _programLoader.Join();
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
    }
}