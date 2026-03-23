using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using DosboxLauncher.Loader;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
        Opened += OnOpened;
        Closed += OnClosed;
    }

    public ICollection<Program> Programs { get; } = new ObservableCollection<Program>();

    private void OnProgramLoadedMessageReceived(object? sender, ProgramLoadedMessage e)
    {
        Dispatcher.UIThread.Post(() => Programs.Add(e.Value));
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        Messenger.Register<ProgramLoadedMessage>(this, OnProgramLoadedMessageReceived);
        Messenger.Send(new MainWindowStateChangeMessage(MainWindowState.Opened));
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        Messenger.Send(new MainWindowStateChangeMessage(MainWindowState.Closed));
        Messenger.UnregisterAll(this);
    }
}