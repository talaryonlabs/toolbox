using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// SyncStatus contains information about the currently observed live and desired states of an application.
/// </summary>
public class V1alpha1SyncStatus
{
    public V1alpha1ComparedTo ComparedTo { get; set; }

    /// <summary>
    /// Revision contains information about the revision the comparison has been performed to.
    /// </summary>
    public string Revision { get; set; }

    /// <summary>
    /// Revisions contains information about the revisions of multiple sources the comparison has been performed to.
    /// </summary>
    public List<string> Revisions { get; set; }

    /// <summary>
    /// Status is the sync state of the comparison.
    /// </summary>
    public string Status { get; set; }
}