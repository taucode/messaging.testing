namespace TauCode.Messaging;

internal class BundleHandlerRecord
{
    internal BundleHandlerRecord(Subscription subscription, Type messageHandlerType)
    {
        this.Subscription = subscription;
        this.MessageHandlerType = messageHandlerType;
    }

    internal Subscription Subscription { get; }

    internal Type MessageHandlerType { get; }
}