using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Launch;

public class DosboxStartRequestMessage(Program program) : ValueChangedMessage<Program>(program);