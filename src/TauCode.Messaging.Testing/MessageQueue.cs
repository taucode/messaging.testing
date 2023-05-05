using TauCode.Working.Slavery;

namespace TauCode.Messaging.Testing;

internal class MessageQueue : QueueSlaveBase<IMessage>
{
    private readonly MessageMedia _owner;

    internal MessageQueue(MessageMedia owner)
        : base(owner.Logger)
    {
        _owner = owner;
    }

    protected override async Task DoAssignment(IMessage assignment, CancellationToken cancellationToken)
    {
        await _owner.DispatchMessage(assignment);
    }
}