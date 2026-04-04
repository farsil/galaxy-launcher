using System.ComponentModel;

namespace DosboxLauncher.ViewService;

public interface IViewState : INotifyPropertyChanged
{
    public double Scaling { get; }
}