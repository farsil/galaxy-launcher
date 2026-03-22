using CommunityToolkit.Mvvm.Messaging;

namespace DosboxLauncher.Messaging;

public static class Messenger
{
    public static readonly IMessenger Instance = StrongReferenceMessenger.Default;
}