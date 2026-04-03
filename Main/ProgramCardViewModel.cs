using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DosboxLauncher.Launch;

namespace DosboxLauncher.Main;

public sealed class ProgramCardViewModel : ObservableObject
{
    private readonly IDosboxState _dosboxState;

    public ProgramCardViewModel(Program program, IDosboxState dosboxState)
    {
        Program = program;
        _dosboxState = dosboxState;
        _dosboxState.PropertyChanged += OnDosboxStatePropertyChanged;
    }

    public bool IsActive => _dosboxState is { IsRunnable: true, IsActive: false };
    public Program Program { get; }
    public ICommand? Command { get; init; }

    private void OnDosboxStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsActive));
    }
}