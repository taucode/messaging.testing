using TauCode.Working.Slavery;

namespace TauCode.Messaging;

public interface IPublisher : ISlave
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = default);
}