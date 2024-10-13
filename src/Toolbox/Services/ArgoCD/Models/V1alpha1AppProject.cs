using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1AppProject
{
    public V1ObjectMeta Metadata { get; set; }
    public V1alpha1AppProjectSpec Spec { get; set; }
    public V1alpha1AppProjectStatus Status { get; set; }
}

public class V1alpha1AppProjectSpec
{
    public List<V1GroupKind> ClusterResourceBlacklist { get; set; }
    public List<V1GroupKind> ClusterResourceWhitelist { get; set; }
    public string Description { get; set; }
    public List<V1alpha1ApplicationDestination> Destinations { get; set; }
    public List<V1GroupKind> NamespaceResourceBlacklist { get; set; }
    public List<V1GroupKind> NamespaceResourceWhitelist { get; set; }
    public V1alpha1OrphanedResourcesMonitorSettings OrphanedResources { get; set; }
    public bool PermitOnlyProjectScopedClusters { get; set; }
    public List<V1alpha1ProjectRole> Roles { get; set; }
    public List<V1alpha1SignatureKey> SignatureKeys { get; set; }
    public List<string> SourceNamespaces { get; set; }
    public List<string> SourceRepos { get; set; }
    public List<V1alpha1SyncWindow> SyncWindows { get; set; }
}

public class V1alpha1AppProjectStatus
{
    public Dictionary<string, V1alpha1JWTTokens> JwtTokensByRole { get; set; }
}

public class V1alpha1ProjectRole
{
    public string Description { get; set; }
    public List<string> Groups { get; set; }
    public List<V1alpha1JWTToken> JwtTokens { get; set; }
    public string Name { get; set; }
    public List<string> Policies { get; set; }
}

