using System;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Talaryon.Toolbox.Data;

namespace Talaryon.Toolbox;

public static class TalaryonHelper
{
    [Pure]
    public static byte[] SerializeObject<T>(T obj)
    {
        var p = JsonSerializer.Serialize(obj);
        return Encoding.UTF8.GetBytes(p);
    }

    [Pure]
    public static T? DeserializeObject<T>(byte[] data)
    {
        var p = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<T>(p);
    }

    // ReSharper disable once InconsistentNaming
    [Pure]
    public static string UUID()
    {
        return Guid.NewGuid().ToString();
    }

    [Pure]
    public static ulong ParseNamedSize(string namedSize)
    {
        if (string.IsNullOrWhiteSpace(namedSize))
            throw new ArgumentException("Input cannot be null or empty.", nameof(namedSize));

        var suffixes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            // SI-Standard
            { "K", 1 }, { "KB", 1 },
            { "M", 2 }, { "MB", 2 },
            { "G", 3 }, { "GB", 3 },
            { "T", 4 }, { "TB", 4 },
            { "P", 5 }, { "PB", 5 },
            { "E", 6 }, { "EB", 6 },
            // IEC-Standard
            { "Ki", 1 }, { "KiB", 1 },
            { "Mi", 2 }, { "MiB", 2 },
            { "Gi", 3 }, { "GiB", 3 },
            { "Ti", 4 }, { "TiB", 4 },
            { "Pi", 5 }, { "PiB", 5 },
            { "Ei", 6 }, { "EiB", 6 },
        };

        foreach (var (suffix, exponent) in suffixes)
        {
            if (!namedSize.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                continue;

            var numericPart = suffix.Length == 1
                ? namedSize[..^1] // "K", "M", etc.
                : namedSize[..^2]; // "KB", "MiB", etc.

            return ulong.TryParse(numericPart, out var value)
                ? value * (ulong)Math.Pow(1024, exponent)
                : throw new FormatException($"Invalid numeric format in '{namedSize}'.");
        }

        return ulong.Parse(namedSize);
    }
    
    [Pure]
    public static string FormatNamedSize(ulong bytes, string format = "0.##")
    {
        string[] suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB"];
        var suffixIndex = 0;
        double size = bytes;

        while (size >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            size /= 1024;
            suffixIndex++;
        }

        return $"{size.ToString(format)} {suffixes[suffixIndex]}";
    }

    [Pure]
    public static TimeSpan ParseNamedDelay(string namedDelay)
    {
        var delays = new[] { "ms", "d", "h", "m", "s" };

        foreach (var delay in delays)
            if (namedDelay.EndsWith(delay, true, null))
            {
                var value = namedDelay[..^delay.Length];
                var number = double.Parse(value);

                return delay switch
                {
                    "d" => TimeSpan.FromDays(number),
                    "h" => TimeSpan.FromHours(number),
                    "m" => TimeSpan.FromMinutes(number),
                    "s" => TimeSpan.FromSeconds(number),
                    "ms" => TimeSpan.FromMilliseconds(number),
                    _ => throw new FormatException()
                };
            }

        return TimeSpan.FromSeconds(double.Parse(namedDelay));
    }

    [Pure]
    public static (string? protocol, string hostname, short port) ParseHostname(string hostname)
    {
        var position = 0;
        var protocol = default(string);
        var port = (short)-1;


        if (hostname.Contains("://"))
        {
            position = hostname.IndexOf("://", StringComparison.Ordinal);
            protocol = hostname.Substring(0, position);
            hostname = hostname.Substring(position + 3, hostname.Length - 3 - position);
        }

        if (hostname.Contains(":"))
        {
            position = hostname.IndexOf(":", StringComparison.Ordinal) + 1;
            port = short.Parse(hostname.Substring(position, hostname.Length - position));
            hostname = hostname.Substring(0, position - 1);
        }

        return (protocol, hostname, port);
    }

    [Pure]
    public static ConnectionString ParseConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

        var result = new ConnectionString();
        var remaining = connectionString;

        // Parse type (required)
        var typeEnd = remaining.IndexOf("://", StringComparison.Ordinal);
        if (typeEnd < 0)
            throw new FormatException("Connection string must contain '://' to separate type from the rest.");

        result.Type = remaining.Substring(0, typeEnd);
        remaining = remaining.Substring(typeEnd + 3);

        // Parse auth (username:password@) if present
        var atIndex = remaining.IndexOf('@');
        if (atIndex >= 0)
        {
            var authPart = remaining.Substring(0, atIndex);
            var colonIndex = authPart.IndexOf(':');
            if (colonIndex >= 0)
            {
                result.Username = authPart.Substring(0, colonIndex);
                result.Password = authPart.Substring(colonIndex + 1);
            }
            else
            {
                result.Username = authPart;
            }
            remaining = remaining.Substring(atIndex + 1);
        }

        // Parse host:port[/endpoint][?options]
        var slashIndex = remaining.IndexOf('/');
        var queryIndex = remaining.IndexOf('?');

        // Determine the end of host:port section
        var hostPortEnd = Math.Min(
            slashIndex >= 0 ? slashIndex : int.MaxValue,
            queryIndex >= 0 ? queryIndex : int.MaxValue
        );

        var hostPortPart = hostPortEnd == int.MaxValue
            ? remaining
            : remaining.Substring(0, hostPortEnd);

        // Parse host and port
        var hostColonIndex = hostPortPart.IndexOf(':');
        if (hostColonIndex >= 0)
        {
            result.Host = hostPortPart.Substring(0, hostColonIndex);
            if (int.TryParse(hostPortPart.Substring(hostColonIndex + 1), out var port))
                result.Port = port;
        }
        else
        {
            result.Host = hostPortPart;
        }

        // Parse endpoint if present
        if (slashIndex >= 0 && (queryIndex < 0 || slashIndex < queryIndex))
        {
            var endpointStart = slashIndex + 1;
            var endpointEnd = queryIndex >= 0 ? queryIndex : remaining.Length;
            result.Endpoint = remaining.Substring(endpointStart, endpointEnd - endpointStart);
        }

        // Parse options if present
        if (queryIndex >= 0)
        {
            var optionsString = remaining.Substring(queryIndex + 1);
            var options = ImmutableDictionary.CreateBuilder<string, string>();

            foreach (var pair in optionsString.Split('&'))
            {
                var split = pair.Split('=', 2);
                var key = split.Length > 0 ? Uri.UnescapeDataString(split[0]) : string.Empty;
                var value = split.Length > 1 ? Uri.UnescapeDataString(split[1]) : string.Empty;
                if (!string.IsNullOrEmpty(key))
                    options.Add(key, value);
            }

            result.Options = options.ToImmutable();
        }

        return result;
    }

    /*[Pure]
    public static string ToQueryString<T>(T data)
    {
        return string.Join("&", typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(v => v.CanRead)
            .Select(v =>
            {
                var attr = v.GetCustomAttributes<QueryMemberAttribute>().FirstOrDefault();
                var name = attr is not null ? attr.Name : v.Name;
                var value = v.GetValue(data) ?? "";

                return $"{name.ToLower()}={HttpUtility.UrlEncode(value.ToString())}";
            }));
    }*/

    public static X509Certificate2 CreateSelfSignedCertificate()
    {
        using var rsa = RSA.Create();
        var certificateRequest =
            new CertificateRequest("CN=localhost", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        certificateRequest.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(
                certificateAuthority: false,
                hasPathLengthConstraint: false,
                pathLengthConstraint: 0,
                critical: true
            )
        );
        certificateRequest.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                keyUsages:
                X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment |
                X509KeyUsageFlags.CrlSign | X509KeyUsageFlags.KeyCertSign,
                critical: false
            )
        );
        certificateRequest.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension([new("1.3.6.1.5.5.7.3.1")],
                false));

        certificateRequest.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(
                key: certificateRequest.PublicKey,
                critical: false
            )
        );

        var sanBuilder = new SubjectAlternativeNameBuilder();
        sanBuilder.AddDnsName("localhost");
        certificateRequest.CertificateExtensions.Add(sanBuilder.Build());

        var cert = certificateRequest.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(5));

        // windows only
        return X509CertificateLoader.LoadPkcs12(cert.Export(X509ContentType.Pfx), null,
            X509KeyStorageFlags.Exportable);
    }
}