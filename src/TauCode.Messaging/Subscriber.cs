using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TauCode.Working.Slavery;

namespace TauCode.Messaging;

// todo clean, regions
public abstract class Subscriber : SlaveBase, ISubscriber
{
    #region Fields

    private readonly Dictionary<BundleTag, Bundle> _bundles;
    private readonly SemaphoreSlim _asyncLock;

    #endregion

    #region ctor

    protected Subscriber(IServiceProvider serviceProvider)
        : base(serviceProvider.GetService<ILogger>())
    {
        this.ServiceProvider = serviceProvider;
        _bundles = new Dictionary<BundleTag, Bundle>();
        _asyncLock = new SemaphoreSlim(1, 1);
    }

    #endregion

    #region Abstract

    protected abstract Task SubscribeAsyncImpl(
        Type messageType,
        string? topic,
        Func<IMessage, CancellationToken, Task> callback,
        CancellationToken cancellationToken);

    #endregion

    #region Overridden

    protected override void OnAfterDisposed()
    {
        _asyncLock.Dispose();
    }

    #endregion

    #region ISubscriber Members

    public IServiceProvider ServiceProvider { get; }

    public async Task<ISubscription> SubscribeAsync(
        Type messageHandlerType,
        string? topic,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messageHandlerType);
        var messageType = ExtractMessageType(messageHandlerType);

        var tag = new BundleTag(messageType, topic);

        var newBundle = false; // todo

        var id = Guid.NewGuid().ToString();
        var subscription = new Subscription(this, messageHandlerType, topic, id);

        await _asyncLock.WaitAsync(cancellationToken);
        try
        {
            if (_bundles.ContainsKey(tag))
            {
                throw new NotImplementedException();
            }
            else
            {
                newBundle = true;
                var bundle = new Bundle(this, tag);

                bundle.AddHandler(subscription, messageHandlerType);

                // todo: deal with failure
                await this.SubscribeAsyncImpl(
                    bundle.Tag.MessageType,
                    topic,
                    bundle.OnMessage,
                    cancellationToken);

                _bundles.Add(tag, bundle);
            }
        }
        finally
        {
            _asyncLock.Release();
        }

        return subscription;
    }

    public IReadOnlyList<ISubscription> GetSubscriptions()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private

    internal static Type ExtractMessageType(Type messageHandlerType)
    {
        ArgumentNullException.ThrowIfNull(messageHandlerType);

        var interfaces = messageHandlerType.GetInterfaces();
        var genericMessageHandlerInterfaces = interfaces
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
            .ToList();

        if (genericMessageHandlerInterfaces.Count != 1)
        {
            throw new NotImplementedException();
        }

        var messageType = genericMessageHandlerInterfaces.Single().GetGenericArguments().Single();
        return messageType;
    }

    #endregion

    internal async Task ProcessMessage(Bundle bundle, IMessage message, CancellationToken cancellationToken)
    {
        var handlerTypes = bundle.MessageHandlerTypes;

        foreach (var handlerType in handlerTypes)
        {
            using var scope = this.ServiceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var handler = (IMessageHandler)scopedServiceProvider.GetRequiredService(handlerType);
            await handler.HandleAsync(message, cancellationToken);
        }
    }
}