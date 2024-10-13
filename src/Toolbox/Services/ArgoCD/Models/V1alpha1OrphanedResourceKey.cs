namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1OrphanedResourceKey
{
    public string Group { get; set; }
    public string Kind { get; set; }
    public string Name { get; set; }
}