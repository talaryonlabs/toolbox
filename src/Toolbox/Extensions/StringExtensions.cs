using System;

namespace TalaryonLabs.Toolbox.Extensions;

public static class StringExtensions
{
    public static byte[] ToBytes(this string p)
    {
        var chars = p.ToCharArray();
        var bytes = Array.ConvertAll(chars, v => (byte)v);

        return bytes;
    }
}