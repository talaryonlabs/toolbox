using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.API.Hosting;

[DataContract]
public class ApiRequest<T>
{
    [JsonPropertyName("items")]
    public IDictionary<string, object?> Items { get; set; } = new Dictionary<string, object?>();

    public static T operator +(T a, ApiRequest<T> b)
    {
        return b + a;
    }
        
    public static T operator +(ApiRequest<T> a, T b)
    {
        var obj = Activator.CreateInstance<T>();
        foreach (var property in typeof(T).GetProperties())
        {
            var attribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (attribute is not null && a.Items.TryGetValue(property.Name, out var value))
            {
                property.SetValue(obj, value);
            }
            else
            {
                property.SetValue(obj, property.GetValue(b));
            }
        }

        return obj;
    }
}