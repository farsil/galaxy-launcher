using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GalaxyLauncher.Main;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (DataContext is MainWindowViewModel vm) vm.IsActive = true;
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);

        if (DataContext is MainWindowViewModel vm) vm.IsActive = false;
    }
}