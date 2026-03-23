using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Loader;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Main;

public sealed class MainWindowViewModel()
    : ObservableRecipient(AppMessenger.Instance), IRecipient<ProgramLoadedMessage>
{
    public ICollection<Program> Programs { get; } = new ObservableCollection<Program>();

    public void Receive(ProgramLoadedMessage message)
    {
        Programs.Add(message.Value);
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        Messenger.Send(new MainWindowActiveChangeMessage(true));
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Messenger.Send(new MainWindowActiveChangeMessage(false));
    }
}