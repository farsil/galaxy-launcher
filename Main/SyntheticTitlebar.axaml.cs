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

    private IViewState? _viewState;

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
        _viewState = serviceProvider?.GetRequiredService<IViewState>();

        _viewState?.ExtendClientAreaHint = true;
        _viewState?.TitleBarHeightHint = TitleBarHeight;
        _viewState?.PropertyChanged += OnWindowStateChanged;
    }

    private void OnMinimizeButtonClick(object? sender, RoutedEventArgs e)
    {
        _viewState?.Minimize();
    }

    private void OnMaximizeToggleButtonClick(object? sender, RoutedEventArgs e)
    {
        _viewState?.ToggleMaximize();
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        _viewState?.Close();
    }

    private void OnWindowStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IViewState.State))
            MaximizeToggleButton.Content = _viewState?.State == WindowState.Maximized ? "🗗" : "🗖";
    }
}