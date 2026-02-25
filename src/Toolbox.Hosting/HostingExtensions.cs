using System.Diagnostics.Contracts;
using System.Reflection;
using System.Web;
using Talaryon.Toolbox.Hosting.Api.Attributes;

namespace Talaryon.Toolbox.Hosting;

public static class HostingExtensions
{
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
}