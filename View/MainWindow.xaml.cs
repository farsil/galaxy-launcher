using System.Windows;
using DosboxLauncher.ViewModel;

namespace DosboxLauncher.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        Loaded += OnLoaded;
        Closed += OnClosed;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ((MainWindowViewModel)DataContext).IsActive = true;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        ((MainWindowViewModel)DataContext).IsActive = false;
    }
}