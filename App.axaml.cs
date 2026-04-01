using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DosboxLauncher.Launch;
using DosboxLauncher.Main;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Startup;

public class App : Application
{
    private readonly DosboxRunner _dosboxRunner = new(AppContext.BaseDirectory);
    private readonly ProgramLoader _programLoader = new(AppContext.BaseDirectory);

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
            AppMessenger.Register<DosboxStartRequestMessage>(this, OnDosboxStartRequestMessageReceived);
            desktop.Exit += OnDesktopExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _dosboxRunner.Kill();
        AppMessenger.UnregisterAll(this);
    }

    private void OnDosboxStartRequestMessageReceived(object recipient, DosboxStartRequestMessage message)
    {
        _dosboxRunner.Start(message.Program);
    }

    private void OnMainWindowActiveChangeMessageReceived(object recipient, MainWindowActiveChangeMessage message)
    {
        if (message.IsActive)
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