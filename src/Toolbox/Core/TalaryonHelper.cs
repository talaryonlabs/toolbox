using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Talaryon.Toolbox.API;

namespace Talaryon.Toolbox;

public static class TalaryonHelper
{
    [Pure]
    public static byte[] SerializeObject<T>(T obj)
    {
        var p = JsonConvert.SerializeObject(obj);
        return Encoding.UTF8.GetBytes(p);
    }

    [Pure]
    public static T? DeserializeObject<T>(byte[] data)
    {
        var p = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(p);
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
        var names = new[] { "K", "M", "G", "T", "P", "E" }; // "Z", "Y"
        var alias = new[] { "KB", "MB", "GB", "TB", "PB", "EB" }; // "ZB", "YB"

        for (var i = 0; i < names.Length; i++)
            if (namedSize.EndsWith(names[i], true, null))
            {
                return (ulong)(long.Parse(namedSize.Substring(0, namedSize.Length - 1)) * Math.Pow(1024, i + 1));
            }
            else if (namedSize.EndsWith(alias[i], true, null))
            {
                return (ulong)(long.Parse(namedSize[..^2]) * Math.Pow(1024, i + 1));
            }

        return ulong.Parse(namedSize);
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

    [Pure]
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
    }

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
        return new X509Certificate2(cert.Export(X509ContentType.Pfx), (string?)null, X509KeyStorageFlags.Exportable);
    }
}