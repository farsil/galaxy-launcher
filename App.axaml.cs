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
            AppMessenger.Register<MainWindowActiveChangeMessage>(this,
                OnMainWindowActiveChangeMessageReceived);
            desktop.Exit += OnDesktopExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        AppMessenger.UnregisterAll(this);
    }

    private void OnMainWindowActiveChangeMessageReceived(object recipient, MainWindowActiveChangeMessage message)
    {
        if (message.Value)
        {
            _programLoader.Start();
        }
        else
        {
            _programLoader.RequestStop();
            _programLoader.Join();
        }
    }
}