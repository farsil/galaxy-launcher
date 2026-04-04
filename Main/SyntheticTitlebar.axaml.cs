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

    public SyntheticTitlebar()
    {
        InitializeComponent();
        if (OperatingSystem.IsWindows())
        {
            IsVisible = true;
            AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    private IWindowState WindowState
    {
        get
        {
            ArgumentNullException.ThrowIfNull(field);
            return field;
        }
        set;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        var serviceProvider = this.FindAncestorOfType<IServiceProvidingWindow>()?.ServiceProvider;
        if (serviceProvider != null)
        {
            WindowState = serviceProvider.GetRequiredService<IWindowState>();

            WindowState.ExtendClientAreaHint = true;
            WindowState.TitleBarHeightHint = TitleBarHeight;
            WindowState.PropertyChanged += OnWindowStateChanged;
        }
    }

    private void OnMinimizeButtonClick(object? sender, RoutedEventArgs e)
    {
        WindowState.Minimize();
    }

    private void OnMaximizeToggleButtonClick(object? sender, RoutedEventArgs e)
    {
        WindowState.ToggleMaximize();
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        WindowState.Close();
    }

    private void OnWindowStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IWindowState.IsMaximized))
            MaximizeToggleButton.Content = WindowState.IsMaximized ? "🗗" : "🗖";
    }
}