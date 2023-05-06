namespace TauCode.Messaging.EasyNetQ;

public class EasyNetQSubscriber : Subscriber, IEasyNetQSubscriber
{
    public EasyNetQSubscriber(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    #region Overridden

    public override bool IsPausingSupported => true;

    #endregion

    #region IEasyNetQSubscriber Members

    public string ConnectionString
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    #endregion

    protected override Task SubscribeAsyncImpl(
        Type messageType,
        string? topic,
        Func<IMessage, CancellationToken, Task> callback,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}