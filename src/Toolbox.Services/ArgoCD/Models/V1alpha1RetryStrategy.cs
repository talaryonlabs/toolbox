namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1RetryStrategy
{
    public V1alpha1Backoff Backoff { get; set; }
    public long Limit { get; set; }
}