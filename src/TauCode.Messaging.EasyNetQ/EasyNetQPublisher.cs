using Serilog;

namespace TauCode.Messaging.EasyNetQ;

public class EasyNetQPublisher : Publisher, IEasyNetQPublisher
{
    #region ctor

    public EasyNetQPublisher(ILogger? logger)
        : base(logger)
    {
    }

    #endregion

    protected override void InitImpl()
    {
        throw new NotImplementedException();
    }

    protected override void ShutdownImpl()
    {
        throw new NotImplementedException();
    }

    protected override Task PublishImpl(IMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public string? ConnectionString { get; set; }
}