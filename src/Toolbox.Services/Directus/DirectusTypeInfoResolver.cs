using System.Collections.Immutable;
using System.Text.Json.Serialization.Metadata;

namespace Talaryon.Toolbox.Services.Directus;

public static class DirectusTypeInfoResolver
{
    public static void Modify(JsonTypeInfo typeInfo)
    {
        foreach (var property in typeInfo.Properties.ToImmutableList())
        {
            if (property.AttributeProvider is null) continue;
            foreach (DirectusFieldAttribute attribute in property.AttributeProvider.GetCustomAttributes(typeof(DirectusFieldAttribute), true))
            {
                var newProperty =
                    typeInfo.CreateJsonPropertyInfo(property.PropertyType, attribute.Name);
                newProperty.Set = property.Set;
                typeInfo.Properties.Add(newProperty);
            }
        }
    }
}