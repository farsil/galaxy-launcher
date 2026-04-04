using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using AvaloniaWindowState = Avalonia.Controls.WindowState;

namespace DosboxLauncher.ViewService;

public class WindowState : IWindowState
{
    private readonly Window _window;

    public WindowState(Window window)
    {
        _window = window;
        _window.ScalingChanged += OnScalingChanged;
        _window.PropertyChanged += OnPropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public double Scaling => _window.DesktopScaling;

    public bool IsMaximized => _window.WindowState == AvaloniaWindowState.Maximized;

    public bool ExtendClientAreaHint
    {
        set
        {
            _window.ExtendClientAreaToDecorationsHint = value;
            _window.ExtendClientAreaChromeHints =
                value ? ExtendClientAreaChromeHints.NoChrome : ExtendClientAreaChromeHints.Default;
        }
    }

    public double TitleBarHeightHint
    {
        set => _window.ExtendClientAreaTitleBarHeightHint = value;
    }

    public void ToggleMaximize()
    {
        _window.WindowState = _window.WindowState == AvaloniaWindowState.Maximized
            ? AvaloniaWindowState.Normal
            : AvaloniaWindowState.Maximized;
    }

    public void Close()
    {
        _window.Close();
    }

    public void Minimize()
    {
        _window.WindowState = AvaloniaWindowState.Minimized;
    }

    private void OnScalingChanged(object? sender, EventArgs e)
    {
        PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(Scaling)));
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Window.WindowStateProperty)
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(IsMaximized)));
    }
}