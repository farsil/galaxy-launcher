using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Main;

public enum MainWindowState
{
    Opened,
    Closed,
}

public class MainWindowStateChangeMessage(MainWindowState state) : ValueChangedMessage<MainWindowState>(state);