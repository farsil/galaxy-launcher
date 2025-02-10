namespace DosboxLauncher.Model;

public sealed record Program
{
    public required string Title { get; init; }
    public required string Path { get; init; }
}