using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace DosboxLauncher.Main;

public sealed partial class SyntheticTitlebar : UserControl
{
    private const double TitleBarHeight = 40;

    private Window? _window;

    public SyntheticTitlebar()
    {
        InitializeComponent();
        if (OperatingSystem.IsWindows())
        {
            IsVisible = true;
            AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _window = this.FindAncestorOfType<Window>();
        Debug.Assert(_window != null);

        _window.ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        _window.ExtendClientAreaToDecorationsHint = true;
        _window.ExtendClientAreaTitleBarHeightHint = TitleBarHeight;
        _window.PropertyChanged += OnWindowPropertyChanged;
    }

    private void OnMinimizeButtonClick(object? sender, RoutedEventArgs e)
    {
        _window?.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeToggleButtonClick(object? sender, RoutedEventArgs e)
    {
        _window?.WindowState = _window.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        _window?.Close();
    }

    private void OnWindowPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Window.WindowStateProperty)
            MaximizeToggleButton.Content = _window?.WindowState == WindowState.Maximized ? "🗗" : "🗖";
    }
}