using System.ComponentModel;

namespace DosboxLauncher.Launch;

public interface IDosboxState : INotifyPropertyChanged
{
    bool IsActive { get; set; }
    
    bool IsRunnable { get; set; }
}