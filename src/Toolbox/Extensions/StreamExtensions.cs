using Talaryon.Toolbox.Communication;

namespace Talaryon.Toolbox.Extensions;

public static class StreamExtensions
{
    public static T? ReadPacket<T>(this Stream stream) where T:IPacket
    {
        return stream
            .ReadPacketAsync<T>()
            .RunSynchronouslyWithResult();
    }
    
    public static ValueTask<T?> ReadPacketAsync<T>(this Stream stream, CancellationToken cancellationToken = default) where T : IPacket => Packet.ReadFromStreamAsync<T>(stream, cancellationToken);

    public static void WritePacket<T>(this Stream stream, T packet) where T : IPacket
    {
        stream
            .WritePacketAsync(packet)
            .AsTask()
            .RunSynchronously();
    }

    public static ValueTask WritePacketAsync<T>(this Stream stream, T packet,
        CancellationToken cancellationToken = default) where T : IPacket => Packet.WriteToStreamAsync(stream, packet, cancellationToken);
}