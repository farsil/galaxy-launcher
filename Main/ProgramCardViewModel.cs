using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GalaxyLauncher.Launch;

namespace GalaxyLauncher.Main;

public sealed class ProgramCardViewModel(Program program, IDosboxProcess dosboxProcess) : ObservableObject
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

    public bool CanStart => dosboxProcess is { CanStart: true, HasExited: true };

    public Program Program => program;

    public ICommand? StartCommand { get; init; }

    private void OnActivated()
    {
        dosboxProcess.PropertyChanged += HandleDosboxStateChanged;
    }

    private void OnDeactivated()
    {
        dosboxProcess.PropertyChanged -= HandleDosboxStateChanged;
    }

    private void HandleDosboxStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanStart));
    }
}