namespace TauCode.Messaging.Testing;

public interface IMessageMedia : IDisposable
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken);

    Task<IDisposable> SubscribeAsync(
        Type messageType,
        string? topic,
        Func<IMessage, CancellationToken, Task> handler,
        CancellationToken cancellationToken);
}
