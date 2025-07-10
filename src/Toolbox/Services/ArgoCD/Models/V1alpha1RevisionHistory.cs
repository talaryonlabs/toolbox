namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// RevisionHistory contains history information about a previous sync.
/// </summary>
public class V1alpha1RevisionHistory
{
    public DateTime DeployStartedAt { get; set; }

    public DateTime DeployedAt { get; set; }

    /// <summary>
    /// ID is an auto incrementing identifier of the RevisionHistory.
    /// </summary>
    public long Id { get; set; }

    public V1alpha1OperationInitiator InitiatedBy { get; set; }

    /// <summary>
    /// Revision holds the revision the sync was performed against.
    /// </summary>
    public string Revision { get; set; }

    /// <summary>
    /// Revisions holds the revision of each source in sources field the sync was performed against.
    /// </summary>
    public List<string> Revisions { get; set; }

    public V1alpha1ApplicationSource Source { get; set; }

    /// <summary>
    /// Sources is a reference to the application sources used for the sync operation.
    /// </summary>
    public List<V1alpha1ApplicationSource> Sources { get; set; }
}