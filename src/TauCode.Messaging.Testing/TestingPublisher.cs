using Serilog;

namespace TauCode.Messaging.Testing;

public class TestingPublisher : Publisher
{
    private readonly MessageMedia _media;

    public TestingPublisher(IMessageMedia media, ILogger? logger)
        : base(logger)
    {
        _media = (MessageMedia)media;
    }

    protected override void InitImpl()
    {
        //_media.RegisterPublisher(this);
    }

    protected override void ShutdownImpl()
    {
        //_media.UnregisterPublisher(this);
    }

    protected override async Task PublishImpl(IMessage message, CancellationToken cancellationToken)
    {
        await _media.PublishAsync(message, cancellationToken);
    }
}