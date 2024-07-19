using System;

namespace Talaryon.Toolbox.Communication;

[Serializable]
public class PacketTypeConfigurationException : Exception
{
    public PacketTypeConfigurationException()
    {
    }

    public PacketTypeConfigurationException(string message) : base(message)
    {
    }

    public PacketTypeConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }

    // protected PacketTypeConfigurationException(
    //     System.Runtime.Serialization.SerializationInfo info,
    //     System.Runtime.Serialization.StreamingContext context) : base(info, context)
    // {
    // }
}