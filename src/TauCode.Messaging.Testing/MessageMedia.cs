using Serilog;
using System.Text;

namespace TauCode.Messaging.Testing;

// todo clean
public class MessageMedia : IMessageMedia
{
    #region Nested

    private class MediaSubscription : IDisposable
    {
        internal MediaSubscription(string tag, Func<IMessage, CancellationToken, Task> handler)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Tag = tag;
            this.Handler = handler;
        }

        internal string Id { get; }

        internal string Tag { get; }

        internal Func<IMessage, CancellationToken, Task> Handler { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Fields

    //private readonly HashSet<TestingPublisher> _publishers;
    //private readonly HashSet<TestingSubscriber> _subscribers;

    private readonly Dictionary<string, MediaSubscription> _subscriptions;
    private readonly MessageQueue _queue;
    private readonly ILogger? _logger;

    #endregion

    #region ctor

    public MessageMedia(ILogger? logger)
    {
        _logger = logger;

        //_publishers = new HashSet<TestingPublisher>();
        //_subscribers = new HashSet<TestingSubscriber>();

        _subscriptions = new Dictionary<string, MediaSubscription>();
        _queue = new MessageQueue(this);
        _queue.Start();
    }

    #endregion

    #region IMessageMedia Members

    public Task PublishAsync(IMessage message, CancellationToken cancellationToken)
    {
        _queue.AddAssignment(message);
        return Task.CompletedTask;
    }

    public Task<IDisposable> SubscribeAsync(
        Type messageType,
        string? topic,
        Func<IMessage, CancellationToken, Task> handler,
        CancellationToken cancellationToken)
    {
        var tagString = BuildTag(messageType, topic);
        var subscription = new MediaSubscription(tagString, handler);
        _subscriptions.Add(subscription.Id, subscription);

        return Task.FromResult<IDisposable>(subscription);
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Internal

    //internal void RegisterPublisher(TestingPublisher testingPublisher)
    //{
    //    if (_publishers.Contains(testingPublisher))
    //    {
    //        throw new NotImplementedException();
    //    }

    //    _publishers.Add(testingPublisher);
    //}

    //internal bool UnregisterPublisher(TestingPublisher testingPublisher)
    //{
    //    return _publishers.Remove(testingPublisher);
    //}

    internal ILogger? Logger => _logger;

    #endregion

    internal static string BuildTag(Type messageType, string? topic)
    {
        var sb = new StringBuilder();
        sb.Append($"[{messageType.AssemblyQualifiedName}]");
        if (topic != null)
        {
            sb.Append(":");
            sb.Append(topic);
        }

        return sb.ToString();
    }

    internal async Task DispatchMessage(IMessage message)
    {
        var tag = BuildTag(message.GetType(), message.Topic);

        foreach (var pair in _subscriptions)
        {
            var key = pair.Key;
            var value = pair.Value;

            if (value.Tag.StartsWith(tag))
            {
                var handler = value.Handler;
                await handler(message, CancellationToken.None); // todo: cancellation token here.
            }
        }
    }
}
