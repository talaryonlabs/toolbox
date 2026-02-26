namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// SyncStrategy controls the manner in which a sync is performed.
/// </summary>
public class V1alpha1SyncStrategy
{
    public V1alpha1SyncStrategyApply Apply { get; set; }
    public V1alpha1SyncStrategyHook Hook { get; set; }
}

/// <summary>
/// SyncStrategyApply uses `kubectl apply` to perform the apply.
/// </summary>
public class V1alpha1SyncStrategyApply
{
    /// <summary>
    /// Force indicates whether or not to supply the --force flag to `kubectl apply`. 
    /// The --force flag deletes and re-creates the resource when PATCH encounters conflicts and has retried 5 times.
    /// </summary>
    public bool Force { get; set; }
}

/// <summary>
/// SyncStrategyHook will perform a sync using hooks annotations.
/// If no hook annotation is specified, falls back to `kubectl apply`.
/// </summary>
public class V1alpha1SyncStrategyHook
{
    public V1alpha1SyncStrategyApply SyncStrategyApply { get; set; }
}