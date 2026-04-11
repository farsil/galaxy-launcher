using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GalaxyLauncher.Launch;

public interface IDosboxState : INotifyPropertyChanged
{
    bool IsActive { get; set; }

    bool IsRunnable { get; set; }
}

public sealed partial class DosboxState : ObservableObject, IDosboxState
{
    [ObservableProperty] public partial bool IsActive { get; set; }

    [ObservableProperty] public partial bool IsRunnable { get; set; }
}