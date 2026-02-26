namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1Operation
{
    public List<V1alpha1Info> Info { get; set; }
    public V1alpha1OperationInitiator InitiatedBy { get; set; }
    public V1alpha1RetryStrategy Retry { get; set; }
    public V1alpha1SyncOperation Sync { get; set; }
}

public class V1alpha1OperationInitiator
{
    public bool Automated { get; set; }
    public string Username { get; set; }
}

public class V1alpha1OperationState
{
    public DateTime FinishedAt { get; set; }
    public string Message { get; set; }
    public V1alpha1Operation Operation { get; set; }
    public string Phase { get; set; }
    public long RetryCount { get; set; }
    public DateTime StartedAt { get; set; }
    public V1alpha1SyncOperationResult SyncResult { get; set; }
}