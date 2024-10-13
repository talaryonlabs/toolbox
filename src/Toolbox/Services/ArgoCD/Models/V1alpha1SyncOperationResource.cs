namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// SyncOperationResource contains resources to sync.
/// </summary>
public class V1alpha1SyncOperationResource
{
    /// <summary>
    /// Group specifies the API group of the resource.
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Kind specifies the API kind of the resource.
    /// </summary>
    public string Kind { get; set; }

    /// <summary>
    /// Name specifies the name of the resource.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Namespace specifies the target namespace of the resource.
    /// </summary>
    public string Namespace { get; set; }
}