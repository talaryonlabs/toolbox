using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
///     SyncPolicy controls when a sync will be performed in response to updates in git.
/// </summary>
public class V1alpha1SyncPolicy
{
    public V1alpha1SyncPolicyAutomated Automated { get; set; }
    public V1alpha1ManagedNamespaceMetadata ManagedNamespaceMetadata { get; set; }
    public V1alpha1RetryStrategy Retry { get; set; }
    public List<string> SyncOptions { get; set; }
}

/// <summary>
///     SyncPolicyAutomated controls the behavior of an automated sync.
/// </summary>
public class V1alpha1SyncPolicyAutomated
{
    /// <summary>
    ///     AllowEmpty allows apps to have zero live resources (default: false).
    /// </summary>
    public bool AllowEmpty { get; set; }

    /// <summary>
    ///     Prune specifies whether to delete resources from the cluster that are not found in the sources anymore as part of
    ///     automated sync (default: false).
    /// </summary>
    public bool Prune { get; set; }

    /// <summary>
    ///     SelfHeal specifies whether to revert resources back to their desired state upon modification in the cluster
    ///     (default: false).
    /// </summary>
    public bool SelfHeal { get; set; }
}