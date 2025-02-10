using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Model;

namespace DosboxLauncher;

public partial class App : Application
{
    public static readonly IMessenger Messenger = StrongReferenceMessenger.Default;
    
    private readonly ProgramLoader _programLoader = new(".");
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Messenger.Register<MainWindowActivationChangeMessage>(this, OnMainWindowActivationChange);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        Messenger.Unregister<MainWindowActivationChangeMessage>(this);
    }

    private void OnMainWindowActivationChange(object recipient, MainWindowActivationChangeMessage message)
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