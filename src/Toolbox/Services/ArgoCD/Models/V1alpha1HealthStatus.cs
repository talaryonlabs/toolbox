namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// HealthStatus contains information about the currently observed health state of an application or resource.
/// </summary>
public class V1alpha1HealthStatus
{
    /// <summary>
    /// Message is a human-readable informational message describing the health status.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Status holds the status code of the application or resource.
    /// </summary>
    public string Status { get; set; }
}