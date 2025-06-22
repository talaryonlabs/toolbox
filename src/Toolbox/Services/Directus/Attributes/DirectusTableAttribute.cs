using System;

namespace Talaryon.Toolbox.Services.Directus;

[AttributeUsage(AttributeTargets.Class)]
public class DirectusTableAttribute(string name, string[]? additionalFields = null) : Attribute
{
    public string Name { get; set; } = name;
    public string[]? AdditionalFields { get; } = additionalFields;
}