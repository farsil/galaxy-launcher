using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DosboxLauncher.Loader;

public sealed record Program
{
    public required string Title { get; init; }
    public required string Path { get; init; }
    public required string ConfigPath { get; init; }
}

public class ProgramLoadedMessage(Program program) : ValueChangedMessage<Program>(program);