using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DosboxLauncher.Launch;

namespace DosboxLauncher.Main;

public sealed class ProgramCardViewModel(Program program, IDosboxState dosboxState) : ObservableObject
{
    public bool IsActive
    {
        set
        {
            if (field == value) return;
            field = value;

            if (value) OnActivated();
            else OnDeactivated();
        }
    }

    public bool CanStart => dosboxState is { IsRunnable: true, IsActive: false };

    public Program Program => program;

    public ICommand? StartCommand { get; init; }

    private void OnActivated()
    {
        dosboxState.PropertyChanged += HandleDosboxStateChanged;
    }

    private void OnDeactivated()
    {
        dosboxState.PropertyChanged -= HandleDosboxStateChanged;
    }

    private void HandleDosboxStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanStart));
    }
}