namespace DosboxLauncher.Main;

public sealed class MainWindowActiveChangeMessage(bool isActive)
{
    public bool IsActive { get; } = isActive;
}