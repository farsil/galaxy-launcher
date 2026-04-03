using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Launch;

namespace DosboxLauncher.Main;

public sealed partial class MainWindowViewModel(IMessenger messenger, IDosboxState dosboxState)
    : ObservableRecipient(messenger), IRecipient<ProgramLoadedMessage>
{
    private readonly ObservableCollection<ProgramCardViewModel> _cardViewModels = [];

    public IDosboxState DosboxState => dosboxState;

    public IEnumerable<ProgramCardViewModel> FilteredProgramCards =>
        string.IsNullOrWhiteSpace(SearchText)
            ? _cardViewModels
            : _cardViewModels.Where(c => c.Program.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

    public string SearchText
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            OnPropertyChanged(nameof(FilteredProgramCards));
        }
    } = string.Empty;

    public void Receive(ProgramLoadedMessage message)
    {
        _cardViewModels.Add(new ProgramCardViewModel(message.Program, dosboxState)
        {
            Command = StartDosboxCommand
        });

        OnPropertyChanged(nameof(FilteredProgramCards));
    }

    [RelayCommand]
    private void StartDosbox(Program program)
    {
        Messenger.Send(new DosboxStartRequestMessage(program));
    }

    [RelayCommand]
    private void StopDosbox()
    {
        Messenger.Send(new DosboxStopRequestMessage());
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        Messenger.Send(new ProgramLoaderStartRequestMessage());
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Messenger.Send(new ProgramLoaderStopRequestMessage());
    }
}