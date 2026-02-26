namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// ResourceStatus holds the current sync and health status of a resource.
/// </summary>
public class V1alpha1ResourceStatus
{
    public string Group { get; set; }
    public V1alpha1HealthStatus Health { get; set; }
    public bool Hook { get; set; }
    public string Kind { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; }
    public bool RequiresPruning { get; set; }
    public string Status { get; set; }
    public long SyncWave { get; set; }
    public string Version { get; set; }
}