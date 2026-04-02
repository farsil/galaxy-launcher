using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace DosboxLauncher.Main;

public sealed partial class SyntheticTitlebar : UserControl
{
    private const double TitleBarHeight = 40;

    public SyntheticTitlebar()
    {
        InitializeComponent();
        if (OperatingSystem.IsWindows())
        {
            IsVisible = true;
            AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    private Window Window
    {
        get
        {
            field ??= this.FindAncestorOfType<Window>();
            return field ?? throw new InvalidOperationException("Component not yet attached to the visual tree");
        }
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        Window.PropertyChanged += OnWindowStateChanged;
        Window.ExtendClientAreaToDecorationsHint = true;
        Window.ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        Window.ExtendClientAreaTitleBarHeightHint = TitleBarHeight;
    }

    private void OnMinimizeButtonClick(object? sender, RoutedEventArgs e)
    {
        Window.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeToggleButtonClick(object? sender, RoutedEventArgs e)
    {
        Window.WindowState = Window.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        Window.Close();
    }

    private void OnWindowStateChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Window.WindowStateProperty)
            MaximizeToggleButton.Content = Window.WindowState == WindowState.Maximized ? "🗗" : "🗖";
    }
}