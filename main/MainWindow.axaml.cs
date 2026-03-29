using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainWindowViewModel();
        Opened += OnOpened;
        Closed += OnClosed;

        // Custom window chrome for Windows systems
        if (OperatingSystem.IsWindows())
        {
            // We only need it for the custom window chrome
            PropertyChanged += OnPropertyChanged;
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
            ExtendClientAreaTitleBarHeightHint = 40;
            SyntheticTitlebar.IsVisible = true;
            MainPanel.Margin = new Thickness(
                MainPanel.Margin.Left,
                40,
                MainPanel.Margin.Right,
                MainPanel.Margin.Bottom
            );
        }
    }

    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

    private void OnOpened(object? sender, EventArgs e)
    {
        ViewModel.IsActive = true;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        ViewModel.IsActive = false;
    }

    private void OnMinimizeButtonClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnMaximizeToggleButtonClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == WindowStateProperty)
            MaximizeToggleButton.Content = WindowState == WindowState.Maximized ? "🗗" : "🗖";
    }
}