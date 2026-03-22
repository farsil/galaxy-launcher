using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Loader;

public class ProgramLoadedMessage(Program program) : ValueChangedMessage<Program>(program);