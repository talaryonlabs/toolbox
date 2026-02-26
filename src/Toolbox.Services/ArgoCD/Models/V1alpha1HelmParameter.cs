namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1HelmParameter
{
    public bool ForceString { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
}