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
    
    public MainWindowViewModel(IMessenger messenger) : base(messenger)
    {
        BindingOperations.EnableCollectionSynchronization(Programs, _programsLock);
    }

    public void Receive(ProgramLoadedMessage message)
    {
        lock (_programsLock)
        {
            Programs.Add(message.Value);
        }
    }
}