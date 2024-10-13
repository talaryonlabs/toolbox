namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// ResourceResult holds the operation result details of a specific resource.
/// </summary>
public class V1alpha1ResourceResult
{
    /// <summary>
    /// Group specifies the API group of the resource.
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// HookPhase contains the state of any operation associated with this resource OR hook. This can also contain values for non-hook resources.
    /// </summary>
    public string HookPhase { get; set; }

    /// <summary>
    /// HookType specifies the type of the hook. Empty for non-hook resources.
    /// </summary>
    public string HookType { get; set; }

    /// <summary>
    /// Kind specifies the API kind of the resource.
    /// </summary>
    public string Kind { get; set; }

    /// <summary>
    /// Message contains an informational or error message for the last sync OR operation.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Name specifies the name of the resource.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Namespace specifies the target namespace of the resource.
    /// </summary>
    public string Namespace { get; set; }

    /// <summary>
    /// Status holds the final result of the sync. Will be empty if the resources is yet to be applied/pruned and is always zero-value for hooks.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// SyncPhase indicates the particular phase of the sync that this result was acquired in.
    /// </summary>
    public string SyncPhase { get; set; }

    /// <summary>
    /// Version specifies the API version of the resource.
    /// </summary>
    public string Version { get; set; }
}