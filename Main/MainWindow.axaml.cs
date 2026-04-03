using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += OnOpened;
        Closed += OnClosed;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        if (DataContext is ObservableRecipient o) o.IsActive = true;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (DataContext is ObservableRecipient o) o.IsActive = false;
    }
}