using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using DosboxLauncher.ViewService;
using Microsoft.Extensions.DependencyInjection;

namespace DosboxLauncher.Main;

public sealed partial class SyntheticTitlebar : UserControl
{
    private const double TitleBarHeight = 40;

    private IWindowState? _windowState;

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
        var serviceProvider = this.FindAncestorOfType<IServiceProvider>();
        _windowState = serviceProvider?.GetRequiredService<IWindowState>();

        _windowState?.ExtendClientAreaHint = true;
        _windowState?.TitleBarHeightHint = TitleBarHeight;
        _windowState?.PropertyChanged += OnWindowStateChanged;
    }

    private void OnMinimizeButtonClick(object? sender, RoutedEventArgs e)
    {
        _windowState?.Minimize();
    }

    private void OnMaximizeToggleButtonClick(object? sender, RoutedEventArgs e)
    {
        _windowState?.ToggleMaximize();
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        _windowState?.Close();
    }

    private void OnWindowStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IWindowState.IsMaximized))
            MaximizeToggleButton.Content = _windowState?.IsMaximized == true ? "🗗" : "🗖";
    }
}