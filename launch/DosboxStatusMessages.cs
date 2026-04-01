namespace DosboxLauncher.Launch;

public class DosboxStartRequestMessage(Program program)
{
    public Program Program { get; } = program;
}

public class DosboxActiveChangeMessage(bool isDosboxActive)
{
    public bool IsDosboxActive { get; } = isDosboxActive;
}