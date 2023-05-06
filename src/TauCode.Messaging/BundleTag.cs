using System.Text;

namespace TauCode.Messaging;

internal readonly struct BundleTag : IEquatable<BundleTag>
{
    internal BundleTag(Type messageType, string? topic)
    {
        MessageType = messageType;
        Topic = topic;
    }

    internal Type MessageType { get; }
    internal string? Topic { get; }

    public bool Equals(BundleTag other)
    {
        return MessageType == other.MessageType && Topic == other.Topic;
    }

    public override bool Equals(object? obj)
    {
        return obj is BundleTag other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MessageType, Topic);
    }

    public override string ToString() => Build(this.MessageType, this.Topic);

    // todo: need this?!
    internal static string Build(Type messageType, string? topic)
    {
        var sb = new StringBuilder();
        sb.Append($"[{messageType.AssemblyQualifiedName}]");
        if (topic != null)
        {
            sb.Append(":");
            sb.Append(topic);
        }

        return sb.ToString();
    }
}