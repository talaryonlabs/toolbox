using System;
using System.Text;

namespace Talaryon.Toolbox.Extensions;

public static class StringExtensions
{
    public static byte[] ToBytes(this string p)
    {
        var chars = p.ToCharArray();
        var bytes = Array.ConvertAll(chars, v => (byte)v);

        return bytes;
    }

    public static string ToBase64String(this string p)
    {
        return Convert.ToBase64String(p.ToBytes());
    }

    public static string FromBase64String(this string p)
    {
        var bytes= Convert.FromBase64String(p);
        return Encoding.UTF8.GetString(bytes);
    }
}