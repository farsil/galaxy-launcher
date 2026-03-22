using CommunityToolkit.Mvvm.Messaging;

namespace DosboxLauncher.Common;

public static class Messenger
{
    public static readonly IMessenger Instance = StrongReferenceMessenger.Default;
}