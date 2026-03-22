using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Messaging;
using DosboxLauncher.Loader;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{

    public ICollection<Program> Programs { get; } = new ObservableCollection<Program>();
    
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = this;
        Opened += OnOpened;
        Closed += OnClosed;
    }

    private void OnProgramLoadedMessageReceived(object? sender, ProgramLoadedMessage e)
    {
        Dispatcher.UIThread.Post(() => Programs.Add(e.Value));
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        Messenger.Instance.Register<ProgramLoadedMessage>(this, OnProgramLoadedMessageReceived);
        Messenger.Instance.Send(new MainWindowStateChangeMessage(MainWindowState.Opened));
    }
    
    private void OnClosed(object? sender, EventArgs e)
    {
        Messenger.Instance.Send(new MainWindowStateChangeMessage(MainWindowState.Closed));
        Messenger.Instance.UnregisterAll(this);
    }
}