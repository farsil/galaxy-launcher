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
    private readonly List<ProgramCardViewModel> _programCardViewModels = [];

    public IDosboxState DosboxState => dosboxState;

    [ObservableProperty] public partial bool HasSearchResults { get; set; }

    public ObservableCollection<ProgramCardViewModel> SearchResults { get; private set; } = [];

    public string? SearchText
    {
        get;
        set
        {
            if (field == value) return;
            field = value;

            // It's more efficient to replace the whole collection and notify the property has changed here
            SearchResults = new ObservableCollection<ProgramCardViewModel>(
                _programCardViewModels.Where(ShouldBeIncludedInSearchResults)
            );
            OnPropertyChanged(nameof(SearchResults));

            HasSearchResults = SearchResults.Count > 0;
        }
    }

    public void Receive(ProgramLoadedMessage message)
    {
        var programCardViewModel = new ProgramCardViewModel(message.Program, dosboxState)
        {
            IsActive = true,
            StartCommand = StartDosboxCommand
        };

        AddProgramCardViewModel(_programCardViewModels, programCardViewModel);

        if (ShouldBeIncludedInSearchResults(programCardViewModel))
        {
            AddProgramCardViewModel(SearchResults, programCardViewModel);
            HasSearchResults = true;
        }
    }

    private bool ShouldBeIncludedInSearchResults(ProgramCardViewModel programCardViewModel)
    {
        return string.IsNullOrWhiteSpace(SearchText) ||
               programCardViewModel.Program.Title.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase);
    }

    private static void AddProgramCardViewModel(IList<ProgramCardViewModel> list, ProgramCardViewModel item)
    {
        // We keep the collection sorted by title, so we don't have to worry about order again
        foreach (var (index, current) in list.Index())
            if (StringComparer.CurrentCultureIgnoreCase.Compare(current.Program.Title, item.Program.Title) > 0)
            {
                list.Insert(index, item);
                return;
            }

        list.Insert(list.Count, item);
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

        foreach (var vm in _programCardViewModels)
            vm.IsActive = true;

        Messenger.Send(new ProgramLoaderStartRequestMessage());
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();

        foreach (var vm in _programCardViewModels)
            vm.IsActive = false;

        Messenger.Send(new ProgramLoaderStopRequestMessage());
    }
}