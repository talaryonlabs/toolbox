using System;

namespace TalaryonLabs.Toolbox.Communication;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public sealed class PacketDataAttribute : Attribute
{
}