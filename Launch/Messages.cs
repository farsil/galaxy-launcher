namespace GalaxyLauncher.Launch;

public sealed class DosboxStartRequestMessage(Program program)
{
    public Program Program { get; } = program;
}

public sealed class DosboxStopRequestMessage;

public sealed class ProgramLoadedMessage(Program program)
{
    public Program Program { get; } = program;
}

public sealed class ProgramLoaderStartRequestMessage;

public sealed class ProgramLoaderStopRequestMessage;