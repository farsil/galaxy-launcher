using System;
using System.ComponentModel;
using Avalonia.Controls;

namespace DosboxLauncher.ViewService;

public class ViewState : IViewState
{
    private readonly Window _window;

    public ViewState(Window window)
    {
        _window = window;
        _window.ScalingChanged += OnScalingChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public double Scaling => _window.DesktopScaling;

    private void OnScalingChanged(object? sender, EventArgs e)
    {
        PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(Scaling)));
    }
}