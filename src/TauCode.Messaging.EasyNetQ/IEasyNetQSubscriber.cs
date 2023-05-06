namespace TauCode.Messaging.EasyNetQ;

public interface IEasyNetQSubscriber : ISubscriber
{
    string ConnectionString { get; set; }
}