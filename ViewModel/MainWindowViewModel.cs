using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Model;

namespace DosboxLauncher.ViewModel;

public class MainWindowViewModel : ObservableRecipient, IRecipient<ProgramLoadedMessage>
{
    private readonly object _programsLock = new();

    public ICollection<Program> Programs { get; } = new ObservableCollection<Program>();
    
    public MainWindowViewModel() : base(App.Messenger)
    {
        BindingOperations.EnableCollectionSynchronization(Programs, _programsLock);
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        Messenger.Send(new MainWindowActivationChangeMessage(true));
    }
    
    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Messenger.Send(new MainWindowActivationChangeMessage(false));
    }
    
    public void Receive(ProgramLoadedMessage message)
    {
        lock (_programsLock)
        {
            Programs.Add(message.Value);
        }
    }
}