using CommunityToolkit.Mvvm.ComponentModel;

namespace DosboxLauncher.Launch;

public sealed partial class DosboxState : ObservableObject, IDosboxState
{
    [ObservableProperty] public partial bool IsActive { get; set; }
    
    [ObservableProperty] public partial bool IsRunnable { get; set; }
}