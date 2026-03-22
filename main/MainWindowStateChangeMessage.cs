using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Main;

public class MainWindowStateChangeMessage(MainWindowState state) : ValueChangedMessage<MainWindowState>(state);