namespace DosboxLauncher.Launch;

public sealed record Program
{
    public required string Title { get; init; }
    public required string Path { get; init; }
    public required string ConfigPath { get; init; }
    public string? ImagePath { get; init; }
}