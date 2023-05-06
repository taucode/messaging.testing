namespace TauCode.Messaging;

public interface IMessage
{
    string? Topic { get; set; }
    string? CorrelationId { get; set; }
    DateTimeOffset CreatedAt { get; set; }
}