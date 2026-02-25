using Talaryon.Toolbox.Communication;

namespace Talaryon.Toolbox.Extensions;

public static class StreamExtensions
{
    extension(Stream stream)
    {
        public T? ReadPacket<T>() where T:IPacket
        {
            return stream
                .ReadPacketAsync<T>()
                .RunSynchronouslyWithResult();
        }

        public ValueTask<T?> ReadPacketAsync<T>(CancellationToken cancellationToken = default) where T : IPacket => Packet.ReadFromStreamAsync<T>(stream, cancellationToken);

        public void WritePacket<T>(T packet) where T : IPacket
        {
            stream
                .WritePacketAsync(packet)
                .AsTask()
                .RunSynchronously();
        }

        public ValueTask WritePacketAsync<T>(T packet,
            CancellationToken cancellationToken = default) where T : IPacket => Packet.WriteToStreamAsync(stream, packet, cancellationToken);
    }
}