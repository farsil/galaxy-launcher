using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Main;

public class MainWindowActiveChangeMessage(bool active) : ValueChangedMessage<bool>(active);