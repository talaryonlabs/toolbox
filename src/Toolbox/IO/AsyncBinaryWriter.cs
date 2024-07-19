using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Talaryon.Toolbox.IO;

public class AsyncBinaryWriter(Stream stream)
{
    public Stream BaseStream => stream;
    
    public Task WriteAsync(byte value, CancellationToken cancellationToken = default) => stream.WriteAsync([value], 0, 1, cancellationToken);
    public Task WriteAsync(bool value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 1, cancellationToken);

    public Task WriteAsync(ushort value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 2, cancellationToken);
    public Task WriteAsync(short value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 2, cancellationToken);

    public Task WriteAsync(uint value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 4, cancellationToken);
    public Task WriteAsync(int value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 4, cancellationToken);

    public Task WriteAsync(ulong value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 8, cancellationToken);
    public Task WriteAsync(long value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 8, cancellationToken);

    public Task WriteAsync(float value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 4, cancellationToken);

    public Task WriteAsync(double value, CancellationToken cancellationToken = default) => stream.WriteAsync(BitConverter.GetBytes(value), 0, 8, cancellationToken);
    public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default) => stream.WriteAsync(buffer, offset, count, cancellationToken);
}