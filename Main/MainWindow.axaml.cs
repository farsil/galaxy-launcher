using System;
using Avalonia.Controls;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += OnOpened;
        Closed += OnClosed;
    }

    public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel ?? throw new InvalidCastException();

    private void OnOpened(object? sender, EventArgs e)
    {
        ViewModel.IsActive = true;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        ViewModel.IsActive = false;
    }
}