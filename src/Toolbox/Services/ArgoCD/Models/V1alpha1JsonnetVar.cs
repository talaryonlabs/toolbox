namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// JsonnetVar represents a variable to be passed to jsonnet during manifest generation.
/// </summary>
public class V1alpha1JsonnetVar
{
    public bool Code { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}