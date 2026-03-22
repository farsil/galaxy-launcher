using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Messaging;
using DosboxLauncher.Loader;
using DosboxLauncher.Main;

namespace DosboxLauncher.Startup;

public partial class App : Application
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
            Messenger.Instance.Register<MainWindowStateChangeMessage>(this, OnMainWindowStateChangeMessageReceived);
            desktop.Exit += OnDesktopExit; 
        }
        
        base.OnFrameworkInitializationCompleted();
    }
    
    private void OnDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Messenger.Instance.UnregisterAll(this);
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