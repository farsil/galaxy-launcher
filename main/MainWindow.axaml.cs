using System;
using Avalonia.Controls;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainWindowViewModel();
        Opened += OnOpened;
        Closed += OnClosed;
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
}