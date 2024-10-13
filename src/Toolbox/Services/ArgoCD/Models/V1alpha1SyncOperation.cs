using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1SyncOperation
{
    public bool DryRun { get; set; }
    public List<string> Manifests { get; set; }
    public bool Prune { get; set; }
    public List<V1alpha1SyncOperationResource> Resources { get; set; }
    public string Revision { get; set; }
    public List<string> Revisions { get; set; }
    public V1alpha1ApplicationSource Source { get; set; }
    public List<V1alpha1ApplicationSource> Sources { get; set; }
    public List<string> SyncOptions { get; set; }
    public V1alpha1SyncStrategy SyncStrategy { get; set; }
}

public class V1alpha1SyncOperationResult
{
    public V1alpha1ManagedNamespaceMetadata ManagedNamespaceMetadata { get; set; }
    public List<V1alpha1ResourceResult> Resources { get; set; }
    public string Revision { get; set; }
    public List<string> Revisions { get; set; }
    public V1alpha1ApplicationSource Source { get; set; }
    public List<V1alpha1ApplicationSource> Sources { get; set; }
}