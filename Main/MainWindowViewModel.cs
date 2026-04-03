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

    public IEnumerable<ProgramCardViewModel> ProgramCards => _cardViewModels
        .Where(c => string.IsNullOrWhiteSpace(SearchText) ||
                    c.Program.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
        .OrderBy(c => c.Program.Title);

    public string? SearchText
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            OnPropertyChanged(nameof(ProgramCards));
        }
    }

    public void Receive(ProgramLoadedMessage message)
    {
        _cardViewModels.Add(new ProgramCardViewModel(message.Program, dosboxState)
        {
            Command = StartDosboxCommand
        });

        OnPropertyChanged(nameof(ProgramCards));
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