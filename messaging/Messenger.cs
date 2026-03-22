using CommunityToolkit.Mvvm.Messaging;

namespace DosboxLauncher.Messaging;

public static class Messenger
{
    private static readonly StrongReferenceMessenger Instance = StrongReferenceMessenger.Default;
    
    public static TMessage Send<TMessage>(TMessage message)
        where TMessage : class
    {
        return Instance.Send(message);
    }

    public static void Register<TMessage>(object recipient,
        MessageHandler<object, TMessage> handler)
        where TMessage : class
    {
        Instance.Register(recipient, handler);
    }

    public static void UnregisterAll(object recipient)
    {
        Instance.UnregisterAll(recipient);
    }
}