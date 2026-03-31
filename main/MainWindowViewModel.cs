using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Loader;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Main;

public sealed partial class MainWindowViewModel()
    : ObservableRecipient(AppMessenger.Instance), IRecipient<ProgramLoadedMessage>, IRecipient<DosboxFoundMessage>
{
    private readonly ObservableCollection<Program> _programs = [];
    private string _dosboxPath = string.Empty;
    private Process? _dosboxProcess;

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

    public void Receive(DosboxFoundMessage message)
    {
        _dosboxPath = message.Value;
    }

    public void Receive(ProgramLoadedMessage message)
    {
        _programs.Add(message.Value);
        OnPropertyChanged(nameof(FilteredPrograms));
    }

    [RelayCommand]
    private void StartProgram(Program program)
    {
        _dosboxProcess = Process.Start(_dosboxPath, ["--conf", program.ConfigPath, "--working-dir", program.Path]);
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        
        Messenger.Send(new MainWindowActiveChangeMessage(true));
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        
        if (_dosboxProcess?.HasExited == false)
            _dosboxProcess.Kill();
        Messenger.Send(new MainWindowActiveChangeMessage(false));
    }
}