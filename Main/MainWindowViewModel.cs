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
    private readonly ObservableCollection<ProgramCardViewModel> _programCardViewModels = [];

    public IDosboxState DosboxState => dosboxState;

    public bool HasSearchResults => SearchResults.Any();

    public IEnumerable<ProgramCardViewModel> SearchResults => _programCardViewModels
        .Where(vm => string.IsNullOrWhiteSpace(SearchText) ||
                     vm.Program.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
        .OrderBy(vm => vm.Program.Title);

    public string? SearchText
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            OnPropertyChanged(nameof(SearchResults));
            OnPropertyChanged(nameof(HasSearchResults));
        }
    }

    public void Receive(ProgramLoadedMessage message)
    {
        _programCardViewModels.Add(new ProgramCardViewModel(message.Program, dosboxState)
        {
            Command = StartDosboxCommand
        });

        OnPropertyChanged(nameof(SearchResults));
        // Only signal that the property is changed if it's the first program loaded
        if (_programCardViewModels.Count == 1)
            OnPropertyChanged(nameof(HasSearchResults));
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