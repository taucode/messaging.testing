using TauCode.Working.Slavery;

namespace TauCode.Messaging;

public interface ISubscriber : ISlave
{
    IServiceProvider ServiceProvider { get; }

    Task<ISubscription> SubscribeAsync(Type messageHandlerType, string? topic = null, CancellationToken cancellationToken = default);

    IReadOnlyList<ISubscription> GetSubscriptions();
}