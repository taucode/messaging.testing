namespace TauCode.Messaging;

internal class Bundle
{
    private readonly Dictionary<string, BundleHandlerRecord> _handlers;

    private readonly Subscriber _owner;

    internal Bundle(Subscriber owner, BundleTag tag)
    {
        _owner = owner;
        this.Tag = tag;
        _handlers = new Dictionary<string, BundleHandlerRecord>();
    }

    internal BundleTag Tag { get; }

    internal void AddHandler(Subscription subscription, Type handlerType)
    {
        var record = new BundleHandlerRecord(subscription, handlerType);
        _handlers.Add(subscription.Id, record);
    }

    internal IReadOnlyList<Type> MessageHandlerTypes => _handlers.Values.Select(x => x.MessageHandlerType).ToList();

    internal async Task OnMessage(IMessage message, CancellationToken cancellationToken)
    {
        await _owner.ProcessMessage(this, message, cancellationToken);
    }
}