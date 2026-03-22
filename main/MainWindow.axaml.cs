using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Loader;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{
    private readonly IMessenger _messenger;

    public ICollection<Program> Programs { get; } = new ObservableCollection<Program>();
    
    public MainWindow(IMessenger messenger)
    {
        InitializeComponent();
        
        DataContext = this;
        Opened += OnOpened;
        Closed += OnClosed;
        _messenger = messenger;
    }

    private void OnProgramLoadedMessageReceived(object? sender, ProgramLoadedMessage e)
    {
        Dispatcher.UIThread.Post(() => Programs.Add(e.Value));
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        _messenger.Register<ProgramLoadedMessage>(this, OnProgramLoadedMessageReceived);
        _messenger.Send(new MainWindowStateChangeMessage(MainWindowState.Opened));
    }
    
    private void OnClosed(object? sender, EventArgs e)
    {
        _messenger.Send(new MainWindowStateChangeMessage(MainWindowState.Closed));
        _messenger.UnregisterAll(this);
    }
}