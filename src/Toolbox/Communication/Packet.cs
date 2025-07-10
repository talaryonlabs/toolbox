using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Talaryon.Toolbox.IO;

namespace Talaryon.Toolbox.Communication;

public static class Packet
{
    private static readonly PacketReader
        Reader = new();

    private static readonly PacketWriter
        Writer = new();

    static Packet()
    {
        Configure((writer, value) => writer.WriteAsync(value, 0, value.Length), (reader, size) => reader.ReadAsync(size));

        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadByteAsync());
        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadCharAsync());
        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadBooleanAsync());

        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadShortAsync());
        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadIntAsync());
        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadLongAsync());

        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadUShortAsync());
        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadUIntAsync());
        Configure((writer, value) => writer.WriteAsync(value), (reader, _) => reader.ReadULongAsync());
    }

    public static void Configure<T>(Func<AsyncBinaryWriter, T, Task> writing,
        Func<AsyncBinaryReader, int, Task<T>> reading)
    {
        Reader.Configure(reading);
        Writer.Configure(writing);
    }

    public static IEnumerable<byte> ToData<T>(this T packet)
        where T : IPacket
    {
        using var stream = new MemoryStream();

        Writer
            .WriteAsync(stream, packet)
            .Wait();

        return stream.ToArray();
    }

    public static T ToPacket<T>(this byte[] data)
        where T : IPacket
    {
        using var stream = new MemoryStream(data);
        var packet = Activator.CreateInstance<T>();

        Reader
            .ReadAsync<T>(stream, packet)
            .AsTask()
            .Wait();

        return packet;
    }

    /*public static bool Check<T>(IEnumerable<byte> data)
    {
        var dummy = Activator.CreateInstance<T>();
        var member = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(v => v.GetCustomAttribute<PacketHeaderAttribute>()?.HeaderType == PacketHeaderType.Verify)
            .ElementAtOrDefault(0);

        if (member == null)
        {
            throw new PacketVerifyException(
                $"No such attribute {nameof(PacketHeaderAttribute)} with HeaderType {PacketHeaderType.Verify}.");
        }

        return member.MemberType switch
        {
            MemberTypes.Property => ((byte[])typeof(BitConverter)
                    .GetMethod("GetBytes", new[] { (member as PropertyInfo).PropertyType })
                    .Invoke(null, new[] { (member as PropertyInfo).GetValue(dummy) }))
                .Compare(data),
            MemberTypes.Field => ((byte[])typeof(BitConverter)
                    .GetMethod("GetBytes", new[] { (member as FieldInfo).FieldType })
                    .Invoke(null, new[] { (member as FieldInfo).GetValue(dummy) }))
                .Compare(data),
            _ => false
        };
    }*/

    public static async ValueTask<T?> ReadFromStreamAsync<T>(Stream stream,
        CancellationToken cancellationToken = default) where T : IPacket
    {
        return await Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var packet = Activator.CreateInstance<T>();
                try
                {
                    await Reader.ReadAsync(stream, packet);
                    return packet;
                }
                catch (PacketVerifyException ex)
                {
                    TalaryonLogger.Error<IPacket>(ex.Message);
                }
            } 
            return default;
        }, cancellationToken);
    }


    public static async ValueTask WriteToStreamAsync<T>(Stream stream, T packet,
        CancellationToken cancellationToken = default) where T : IPacket
    {
        await Writer.WriteAsync(stream, packet);
    }

    private static IEnumerable<PacketHeaderField> GetHeaderFields<T>() where T : IPacket
    {
        return typeof(T)
            .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(v => v.MemberType is MemberTypes.Field or MemberTypes.Property &&
                        v.GetCustomAttributes<PacketHeaderAttribute>().Any())
            .OrderBy(v => v.GetCustomAttribute<PacketHeaderAttribute>()!.Order)
            .Select(v => new PacketHeaderField(v));
    }

    private static PacketField? GetDataField<T>() where T : IPacket
    {
        return typeof(T)
            .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(v => v.MemberType is MemberTypes.Field or MemberTypes.Property &&
                        v.GetCustomAttributes<PacketDataAttribute>().Any())
            .Select(v => new PacketField(v))
            .FirstOrDefault();
    }

    private class PacketField
    {
        protected readonly MemberInfo MemberInfo;

        public PacketField(MemberInfo memberInfo)
        {
            ArgumentNullException.ThrowIfNull(memberInfo);
            MemberInfo = memberInfo;
        }

        public Type Type
        {
            get
            {
                var headerType = MemberInfo.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)MemberInfo).PropertyType,
                    MemberTypes.Field => ((FieldInfo)MemberInfo).FieldType,
                    _ => throw new NotSupportedException()
                };
                if (headerType.IsEnum)
                {
                    headerType = Enum.GetUnderlyingType(headerType);
                }

                return headerType;
            }
        }

        public object? GetValue<T>(T packet)
        {
            ArgumentNullException.ThrowIfNull(packet);

            return MemberInfo.MemberType switch
            {
                MemberTypes.Property => (MemberInfo as PropertyInfo)?.GetValue(packet),
                _ => (MemberInfo as FieldInfo)!.GetValue(packet)
            };
        }

        public void SetValue<T>(T packet, object value)
        {
            ArgumentNullException.ThrowIfNull(packet);

            var obj = (object)packet;

            if (MemberInfo.MemberType == MemberTypes.Property)
            {
                if (!(MemberInfo as PropertyInfo)!.CanWrite) return;
                (MemberInfo as PropertyInfo)!.SetValue(obj, value);
            }
            else
                (MemberInfo as FieldInfo)!.SetValue(obj, value);
        }
    }

    private class PacketHeaderField(MemberInfo memberInfo) : PacketField(memberInfo)
    {
        public PacketHeaderType HeaderType => MemberInfo.GetCustomAttribute<PacketHeaderAttribute>()!.HeaderType;

        public int HeaderLength
        {
            get
            {
                var length = MemberInfo.GetCustomAttribute<PacketHeaderAttribute>()!.HeaderLength;
                return length == -1 ? Marshal.SizeOf(Type) : length;
            }
        }
    }

    private class PacketReader
    {
        private readonly Dictionary<Type, MulticastDelegate> _configs = new();

        public void Configure<T>(Func<AsyncBinaryReader, int, Task<T>> readingCallback)
        {
            _configs.Remove(typeof(T));
            _configs.Add(typeof(T), readingCallback);
        }

        public async ValueTask ReadAsync<T>(Stream stream, T packet)
            where T : IPacket
        {
            var dummy = Activator.CreateInstance<T>();
            var binaryReader = new AsyncBinaryReader(stream);
            var dataLength = default(int);
            var headerFields = GetHeaderFields<T>();

            foreach (var header in headerFields)
            {
                var value = await ReadAsync(binaryReader, header.Type, header.HeaderLength);
                if (header.HeaderType == PacketHeaderType.Verify)
                {
                    var identifier = (byte[])header.GetValue(dummy);
                    if (!((byte[])value).SequenceEqual(identifier)) throw new PacketVerifyException();
                }
                if (header.HeaderType == PacketHeaderType.DataLength)
                {
                    dataLength = Convert.ToInt32(value);
                }
                header.SetValue(packet, value!);
            }

            if (dataLength > 0)
                await ReadDataAsync(binaryReader, packet, dataLength);
        }

        private async ValueTask ReadDataAsync<T>(AsyncBinaryReader binaryReader, T packet, int length)
            where T : IPacket
        {
            var dataField = GetDataField<T>();
            if (dataField is null) return;

            var data = await ReadAsync(binaryReader, dataField.Type, length);
            dataField.SetValue(packet, data!);
        }

        private async ValueTask<object?> ReadAsync(AsyncBinaryReader binaryReader, Type type, int size)
        {
            if (!_configs.TryGetValue(type, out var multicastDelegate))
                throw new PacketTypeConfigurationException($"Configuration for type {type.FullName} not found.");
            
            var task = (Task)multicastDelegate.DynamicInvoke(binaryReader, size)!;
            await task;

            return task
                    .GetType()
                    .GetProperty("Result")?
                    .GetValue(task);
        }
    }

    private class PacketWriter
    {
        private readonly Dictionary<Type, MulticastDelegate>
            _configs = new();

        public void Configure<T>(Func<AsyncBinaryWriter, T, Task> writingCallback)
        {
            _configs.Remove(typeof(T));
            _configs.Add(typeof(T), writingCallback);
        }

        public async Task WriteAsync<T>(Stream stream, T packet)
            where T : IPacket
        {
            var binaryWriter = new AsyncBinaryWriter(stream);
            var dataLength = default(int);
            var dataField = GetDataField<T>();
            var headerFields = GetHeaderFields<T>();
            var data = dataField?.GetValue(packet);

            foreach (var header in headerFields)
            {
                var value = header.GetValue(packet);
                if (header.HeaderType == PacketHeaderType.DataLength && data is not null)
                {
                    using var s = new MemoryStream();
                    var w = new AsyncBinaryWriter(s);

                    await WriteAsync(w, dataField!.Type, data);

                    dataLength = s.ToArray().Length;
                    value = dataLength;
                }
                await WriteAsync(binaryWriter, header.Type, value);
            }

            if(dataLength > 0)
                await WriteAsync(binaryWriter, dataField!.Type, data);
        }

        private Task WriteAsync(AsyncBinaryWriter writer, Type type, object? value)
        {
            if (!_configs.TryGetValue(type, out var test))
                throw new PacketTypeConfigurationException($"Configuration for type {type.FullName} not found.");

            return (Task)(test.DynamicInvoke(writer, value) ?? throw new InvalidOperationException());
        }
    }
}