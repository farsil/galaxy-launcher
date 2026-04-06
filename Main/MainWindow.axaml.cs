using System;
using Avalonia.Controls;

namespace DosboxLauncher.Main;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (DataContext is MainWindowViewModel vm) vm.IsActive = true;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        if (DataContext is MainWindowViewModel vm) vm.IsActive = false;
    }
}