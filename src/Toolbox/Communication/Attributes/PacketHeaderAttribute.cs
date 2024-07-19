using System;

namespace Talaryon.Toolbox.Communication;

public enum PacketHeaderType : ushort
{
    Default = 0x0,
    Verify = 0x1,
    DataLength = 0x2,
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public sealed class PacketHeaderAttribute(int order) : Attribute
{
    public int Order => order;
    public PacketHeaderType HeaderType { get; set; } = PacketHeaderType.Default;
    public int HeaderLength { get; set; } = -1;
}