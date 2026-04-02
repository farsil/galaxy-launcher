using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Launch;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Main;

public sealed partial class MainWindowViewModel()
    : ObservableRecipient(AppMessenger.Instance), IRecipient<ProgramLoadedMessage>,
        IRecipient<DosboxActiveChangeMessage>
{
    private readonly ObservableCollection<Program> _programs = [];

    [ObservableProperty] public partial bool IsDosboxActive { get; private set; }

    public IEnumerable<Program> FilteredPrograms =>
        string.IsNullOrWhiteSpace(SearchText)
            ? _programs
            : _programs.Where(p => p.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

    public string SearchText
    {
        get;
        set
        {
            if (field == value) return;
            field = value;
            OnPropertyChanged(nameof(FilteredPrograms));
        }
    } = string.Empty;

    public void Receive(DosboxActiveChangeMessage message)
    {
        IsDosboxActive = message.IsDosboxActive;
    }

    public void Receive(ProgramLoadedMessage message)
    {
        _programs.Add(message.Program);
        OnPropertyChanged(nameof(FilteredPrograms));
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