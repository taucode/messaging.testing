namespace TauCode.Messaging;

// todo clean
public interface ISubscription : IDisposable
{
    //string Id { get; }
    //Type MessageType { get; }
    Type MessageHandlerType { get; }
    string? Topic { get; }
}