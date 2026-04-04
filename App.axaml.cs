using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Launch;
using DosboxLauncher.Main;

namespace DosboxLauncher.Startup;

public sealed class App : Application
{
    private readonly DosboxRunner _dosboxRunner;
    private readonly DosboxState _dosboxState;
    private readonly StrongReferenceMessenger _messenger;
    private readonly ProgramLoader _programLoader;

    public App()
    {
        _dosboxState = new DosboxState();
        _messenger = StrongReferenceMessenger.Default;
        _programLoader = new ProgramLoader(AppContext.BaseDirectory, _messenger);
        _dosboxRunner = new DosboxRunner(AppContext.BaseDirectory, _dosboxState);
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
            desktop.Exit += OnDesktopExit;
        }

        _messenger.Register<ProgramLoaderStartRequestMessage>(this, OnProgramLoaderStartRequestMessageReceived);
        _messenger.Register<ProgramLoaderStopRequestMessage>(this, OnProgramLoaderStopRequestMessageReceived);
        _messenger.Register<DosboxStartRequestMessage>(this, OnDosboxStartRequestMessageReceived);
        _messenger.Register<DosboxStopRequestMessage>(this, OnDosboxStopRequestMessageReceived);

        base.OnFrameworkInitializationCompleted();
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

    private void OnDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _dosboxRunner.Kill();
        _messenger.UnregisterAll(this);
    }
}