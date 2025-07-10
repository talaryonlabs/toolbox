using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Talaryon.Toolbox.Services.Directus;

[AttributeUsage(AttributeTargets.Property)]
public class DirectusFieldAttribute(string name, string[]? subfields = null) : JsonContainerAttribute
{
    public string Name { get; } = name;
    public string[]? Subfields { get; } = subfields;
}