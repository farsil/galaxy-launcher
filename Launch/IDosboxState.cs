using System.ComponentModel;

namespace GalaxyLauncher.Launch;

public interface IDosboxState : INotifyPropertyChanged
{
    bool IsActive { get; set; }
    
    bool IsRunnable { get; set; }
}