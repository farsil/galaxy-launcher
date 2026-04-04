using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using DosboxLauncher.ViewService;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window, IServiceProvidingWindow
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += OnOpened;
        Closed += OnClosed;
    }

    public IServiceProvider? ServiceProvider { get; set; }

    private void OnOpened(object? sender, EventArgs e)
    {
        if (DataContext is ObservableRecipient o) o.IsActive = true;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (DataContext is ObservableRecipient o) o.IsActive = false;
    }
}