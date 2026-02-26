namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1ManagedNamespaceMetadata
{
    public Dictionary<string, string> Annotations { get; set; }
    public Dictionary<string, string> Labels { get; set; }
}