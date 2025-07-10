namespace Talaryon.Toolbox.Communication;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public sealed class PacketDataAttribute : Attribute
{
}