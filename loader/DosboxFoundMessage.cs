using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Loader;

public class DosboxFoundMessage(string path) : ValueChangedMessage<string>(path);