using Serilog;
using TauCode.Working.Slavery;

namespace TauCode.Messaging;

public abstract class Publisher : SlaveBase, IPublisher
{
    #region ctor

    protected Publisher(ILogger? logger)
        : base(logger)
    {
    }

    #endregion

    #region Abstract

    protected abstract void InitImpl();

    protected abstract void ShutdownImpl();

    protected abstract Task PublishImpl(IMessage message, CancellationToken cancellationToken);

    #endregion

    #region Overridden

    protected override void OnBeforeStarting()
    {
        this.InitImpl();
    }

    protected override void OnBeforeStopping()
    {
        this.ShutdownImpl();
    }

    public override bool IsPausingSupported => true;

    #endregion

    #region IPublisher Members

    public async Task PublishAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        // todo checks

        this.AllowIfStateIs(nameof(PublishAsync), SlaveState.Running); // todo: consider WorkerState as [Flags], so no 'params' but bit masks

        await this.PublishImpl(message, cancellationToken);
    }


    #endregion
}