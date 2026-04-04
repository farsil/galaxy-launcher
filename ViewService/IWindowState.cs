using System.ComponentModel;

namespace DosboxLauncher.ViewService;

public interface IWindowState : INotifyPropertyChanged
{
    public double Scaling { get; }
    public bool IsMaximized { get; }
    public bool ExtendClientAreaHint { set; }
    public double TitleBarHeightHint { set; }
    public void ToggleMaximize();
    public void Close();
    public void Minimize();
}