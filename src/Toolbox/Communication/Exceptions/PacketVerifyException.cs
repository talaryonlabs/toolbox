using System;

namespace Talaryon.Toolbox.Communication;

[Serializable]
public class PacketVerifyException : Exception
{
    public PacketVerifyException()
    {
    }

    public PacketVerifyException(string message) : base(message)
    {
    }

    public PacketVerifyException(string message, Exception inner) : base(message, inner)
    {
    }

    // protected PacketVerifyException(
    //     System.Runtime.Serialization.SerializationInfo info,
    //     System.Runtime.Serialization.StreamingContext context) : base(info, context)
    // {
    // }
}