using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Model;

public class MainWindowActivationChangeMessage(bool active) : ValueChangedMessage<bool>(active);