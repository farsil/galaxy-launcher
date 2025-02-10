using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Model;

public class ProgramLoadedMessage(Program program) : ValueChangedMessage<Program>(program);