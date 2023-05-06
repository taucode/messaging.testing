namespace TauCode.Messaging;

public static class MessagingExtensions
{
    public static Task SubscribeAsync<T>(
        this ISubscriber subscriber,
        string? topic = null,
        CancellationToken cancellationToken = default)
        where T : IMessageHandler
    {
        return subscriber.SubscribeAsync(typeof(T), topic, cancellationToken);
    }

    public static Type GetMessageType(this ISubscription subscription)
    {
        throw new NotImplementedException();
    }
}