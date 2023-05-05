using Microsoft.Extensions.DependencyInjection;

namespace TauCode.Messaging.Testing;

public class TestingSubscriber : Subscriber
{
    private IMessageMedia? _media;

    public TestingSubscriber(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    protected IMessageMedia Media => _media ??= this.ServiceProvider.GetRequiredService<IMessageMedia>();

    public override bool IsPausingSupported => true;

    protected override void OnStarting()
    {
        base.OnStarting();
    }

    protected override async Task SubscribeAsyncImpl(
        Type messageType,
        string? topic,
        Func<IMessage, CancellationToken, Task> callback, // todo: 'callback' vs 'handler'
        CancellationToken cancellationToken)
    {
        await this.Media.SubscribeAsync(messageType, topic, callback, cancellationToken);
    }
}