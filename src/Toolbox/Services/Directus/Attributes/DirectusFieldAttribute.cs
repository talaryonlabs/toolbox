using System.Reflection;
using Newtonsoft.Json;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Directus;

[AttributeUsage(AttributeTargets.Property)]
public class DirectusFieldAttribute(string name, Type? fieldType = null) : JsonContainerAttribute
{
    public static string[] GetFields(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(v => v.HasCustomAttribute<DirectusFieldAttribute>())
            .Select(v =>
            {
                var field = v.GetCustomAttribute<DirectusFieldAttribute>();
                var builder = new List<string>();
                var name = field!.Name;


                if (field.Subfields is not null) builder.AddRange(field.Subfields!.Select(f => $"{name}.{f}"));
                else builder.Add(name);

                return builder.ToArray();
            })
            .SelectMany(v => v)
            .ToArray();

    private string Name { get; } = name;

    private string[]? Subfields => fieldType is not null ? GetFields(fieldType) : null;
}