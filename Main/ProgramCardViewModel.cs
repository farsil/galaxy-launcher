using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DosboxLauncher.Launch;

namespace DosboxLauncher.Main;

public sealed class ProgramCardViewModel(Program program, IDosboxState dosboxState) : ObservableObject
{
    public Program Program { get; } = program;
    public IDosboxState DosboxState { get; } = dosboxState;
    public ICommand? Command { get; init; }
}