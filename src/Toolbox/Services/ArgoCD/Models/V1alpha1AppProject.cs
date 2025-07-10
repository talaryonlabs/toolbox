using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

[ArgoContainer("project")]
[ArgoEndpoint("api/v1/projects/.id", ArgoEndpointType.Get | ArgoEndpointType.Update | ArgoEndpointType.Delete, typeof(V1alpha1AppProject))]
[ArgoEndpoint("api/v1/projects", ArgoEndpointType.Create, typeof(V1alpha1AppProject))]
public class V1alpha1AppProject
{
    [JsonPropertyName("metadata")] public V1ObjectMeta Metadata { get; set; } = new();
    public V1alpha1AppProjectSpec Spec { get; set; } = new();
    public V1alpha1AppProjectStatus Status { get; set; } = new();
}

[ArgoEndpoint("api/v1/projects", ArgoEndpointType.List, typeof(V1alpha1AppProjectList))]
public class V1alpha1AppProjectList
{
    public List<V1alpha1AppProject> Items { get; set; } = new();
    public V1ListMeta Metadata { get; set; } = new();
}

public class V1alpha1AppProjectSpec
{
    public List<V1GroupKind> ClusterResourceBlacklist { get; set; } = new();
    public List<V1GroupKind> ClusterResourceWhitelist { get; set; } = new();
    public string Description { get; set; }
    public List<V1alpha1ApplicationDestination> Destinations { get; set; } = new();
    public List<V1GroupKind> NamespaceResourceBlacklist { get; set; } = new();
    public List<V1GroupKind> NamespaceResourceWhitelist { get; set; } = new();
    public V1alpha1OrphanedResourcesMonitorSettings OrphanedResources { get; set; } = new();
    public bool PermitOnlyProjectScopedClusters { get; set; }
    public List<V1alpha1ProjectRole> Roles { get; set; } = new();
    public List<V1alpha1SignatureKey> SignatureKeys { get; set; } = new();
    public List<string> SourceNamespaces { get; set; } = new();
    public List<string> SourceRepos { get; set; } = new();
    public List<V1alpha1SyncWindow> SyncWindows { get; set; } = new();
}

public class V1alpha1AppProjectStatus
{
    public Dictionary<string, V1alpha1JWTTokens> JwtTokensByRole { get; set; } = new();
}

public class V1alpha1ProjectRole
{
    public string Description { get; set; }
    public List<string> Groups { get; set; } = new();
    public List<V1alpha1JWTToken> JwtTokens { get; set; } = new();
    public string Name { get; set; }
    public List<string> Policies { get; set; } = new();
}