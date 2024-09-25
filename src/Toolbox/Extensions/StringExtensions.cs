using System;
using System.Globalization;
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
        var bytes = Convert.FromBase64String(p);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string SlugifyString(this string p, int maxLength = 0)
    {
        var normalizedString = p
            // Make lowercase
            .ToLowerInvariant()
            // Normalize the text
            .Normalize(NormalizationForm.FormD);

        var stringBuilder = new StringBuilder();
        var stringLength = normalizedString.Length;
        var prevdash = false;
        var trueLength = 0;

        char c;

        for (var i = 0; i < stringLength; i++)
        {
            c = normalizedString[i];

            switch (CharUnicodeInfo.GetUnicodeCategory(c))
            {
                // Check if the character is a letter or a digit if the character is a
                // international character remap it to an ascii valid character
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    if (c < 128)
                        stringBuilder.Append(c);
                    else
                        stringBuilder.Append(c.RemapInternationalCharToAscii());

                    prevdash = false;
                    trueLength = stringBuilder.Length;
                    break;

                // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OtherPunctuation:
                case UnicodeCategory.MathSymbol:
                    if (!prevdash)
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                        trueLength = stringBuilder.Length;
                    }
                    break;
            }

            // If we are at max length, stop parsing
            if (maxLength > 0 && trueLength >= maxLength)
                break;
        }

        // Trim excess hyphens
        var result = stringBuilder.ToString().Trim('-');

        // Remove any excess character to meet maxlength criteria
        return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
    }
}