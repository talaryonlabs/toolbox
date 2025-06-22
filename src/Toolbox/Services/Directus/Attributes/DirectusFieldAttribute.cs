using System;
using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Directus;

[AttributeUsage(AttributeTargets.Property)]
public class DirectusFieldAttribute(string name) : JsonAttribute
{
    public string Name { get; } = name;
}