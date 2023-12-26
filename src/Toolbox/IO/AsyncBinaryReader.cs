using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TalaryonLabs.Toolbox.IO;

// ReSharper disable MustUseReturnValue
public class AsyncBinaryReader(Stream stream)
{
    public Stream BaseStream => stream;

    public async Task<bool> ReadBooleanAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[1];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToBoolean(buffer, 0);
    }

    public async Task<byte> ReadByteAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[1];
        await ReadAsync(buffer, cancellationToken);
        return buffer[0];
    }

    public async Task<char> ReadCharAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[2];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToChar(buffer, 0);
    }

    public async Task<short> ReadShortAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[2];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToInt16(buffer, 0);
    }

    public async Task<ushort> ReadUShortAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[2];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToUInt16(buffer, 0);
    }

    public async Task<int> ReadIntAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[4];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToInt32(buffer, 0);
    }

    public async Task<uint> ReadUIntAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[4];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToUInt32(buffer, 0);
    }

    public async Task<long> ReadLongAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[8];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToInt64(buffer, 0);
    }

    public async Task<ulong> ReadULongAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[8];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToUInt64(buffer, 0);
    }

    public async Task<float> ReadFloatAsync(CancellationToken cancellationToken = default)
    {
        var buffer = new byte[4];
        await ReadAsync(buffer, cancellationToken);
        return BitConverter.ToSingle(buffer, 0);
    }

    public async Task<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return await stream.ReadAsync(buffer, cancellationToken);
    }

    public async Task<byte[]> ReadAsync(int count, CancellationToken cancellationToken = default)
    {
        var buffer = new byte[count];
        await ReadAsync(buffer, cancellationToken);
        return buffer;
    }
}