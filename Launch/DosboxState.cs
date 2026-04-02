using CommunityToolkit.Mvvm.ComponentModel;

namespace DosboxLauncher.Launch;

public sealed partial class DosboxState(bool isActive) : ObservableObject, IDosboxState
{
    [ObservableProperty] public partial bool IsActive { get; set; } = isActive;
}