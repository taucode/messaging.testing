namespace TauCode.Messaging;

internal class Subscription : ISubscription
{
    #region ctor

    internal Subscription(Subscriber subscriber, Type messageHandlerType, string? topic, string id)
    {
        this.Subscriber = subscriber;
        this.MessageHandlerType = messageHandlerType;
        this.Topic = topic;
        this.Id = id;

        this.MessageType = Subscriber.ExtractMessageType(messageHandlerType);
    }

    #endregion

    #region Internal

    internal Subscriber Subscriber { get; }

    internal string Id { get; }

    internal Type MessageType { get; }

    #endregion

    #region ISubscription Members

    public Type MessageHandlerType { get; }

    public string? Topic { get; }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        throw new NotImplementedException();
    }


    #endregion
}