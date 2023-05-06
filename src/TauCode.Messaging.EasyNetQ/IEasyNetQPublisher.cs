namespace TauCode.Messaging.EasyNetQ;

public interface IEasyNetQPublisher : IPublisher
{
    string? ConnectionString { get; set; }
}