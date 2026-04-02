namespace DosboxLauncher.Launch;

public sealed class DosboxStartRequestMessage(Program program)
{
    public Program Program { get; } = program;
}

public sealed class DosboxStopRequestMessage;

public sealed class DosboxActiveChangeMessage(bool isDosboxActive)
{
    public bool IsDosboxActive { get; } = isDosboxActive;
}

public sealed class ProgramLoadedMessage(Program program)
{
    public Program Program { get; } = program;
}

public sealed class ProgramLoaderStartRequestMessage;

public sealed class ProgramLoaderStopRequestMessage;