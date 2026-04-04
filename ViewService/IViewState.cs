using System.ComponentModel;
using Avalonia.Controls;

namespace DosboxLauncher.ViewService;

public interface IViewState : INotifyPropertyChanged
{
    public double Scaling { get; }
    public WindowState State { get; }

    public bool ExtendClientAreaHint { set; }
    public double TitleBarHeightHint { set; }

    public void ToggleMaximize();

    public void Close();

    public void Minimize();
}