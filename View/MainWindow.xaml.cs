using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.ViewModel;

namespace DosboxLauncher.View;

public partial class MainWindow : Window
{
    private readonly ProgramLoader _programLoader = new(".", StrongReferenceMessenger.Default);
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(StrongReferenceMessenger.Default);
        Loaded += OnLoaded;
        Closed += OnClosed;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ((MainWindowViewModel)DataContext).IsActive = true;
        _programLoader.Start();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        ((MainWindowViewModel)DataContext).IsActive = false;
        _programLoader.RequestStop();
        _programLoader.Join();
    }
}