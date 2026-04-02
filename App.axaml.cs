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
            AppMessenger.Register<ProgramLoaderStartRequestMessage>(this, OnProgramLoaderStartRequestMessageReceived);
            AppMessenger.Register<ProgramLoaderStopRequestMessage>(this, OnProgramLoaderStopRequestMessageReceived);
            AppMessenger.Register<DosboxStartRequestMessage>(this, OnDosboxStartRequestMessageReceived);
            AppMessenger.Register<DosboxStopRequestMessage>(this, OnDosboxStopRequestMessageReceived);
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

    private void OnDosboxStopRequestMessageReceived(object recipient, DosboxStopRequestMessage message)
    {
        _dosboxRunner.Kill();
        _dosboxRunner.WaitForExit();
    }

    private void OnProgramLoaderStartRequestMessageReceived(object recipient, ProgramLoaderStartRequestMessage message)
    {
        _programLoader.Start();
    }

    private void OnProgramLoaderStopRequestMessageReceived(object recipient, ProgramLoaderStopRequestMessage message)
    {
        _programLoader.RequestStop();
        _programLoader.Join();
    }
}