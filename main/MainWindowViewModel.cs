using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Loader;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Main;

public sealed class MainWindowViewModel()
    : ObservableRecipient(AppMessenger.Instance), IRecipient<ProgramLoadedMessage>
{
    private readonly ObservableCollection<Program> _programs = [];

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

    public void Receive(ProgramLoadedMessage message)
    {
        _programs.Add(message.Value);
        OnPropertyChanged(nameof(FilteredPrograms));
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