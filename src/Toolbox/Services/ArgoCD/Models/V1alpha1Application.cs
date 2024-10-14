using System;
using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

[ArgoEndpoint("api/v1/applications/.id", ArgoEndpointType.Get | ArgoEndpointType.Update, typeof(V1alpha1Application))]
[ArgoEndpoint("api/v1/applications", ArgoEndpointType.Create, typeof(V1alpha1Application))]
public class V1alpha1Application
{
    public V1ObjectMeta Metadata { get; set; } = new();
    public V1alpha1Operation Operation { get; set; } = new();
    public V1alpha1ApplicationSpec Spec { get; set; } = new();
    public V1alpha1ApplicationStatus Status { get; set; } = new();
}

/// <summary>
/// ApplicationList is a list of Application resources.
/// </summary>
[ArgoEndpoint("api/v1/applications", ArgoEndpointType.List, typeof(V1alpha1ApplicationList))]
public class V1alpha1ApplicationList
{
    /// <summary>
    /// Items is the list of Application resources.
    /// </summary>
    public List<V1alpha1Application> Items { get; set; }

    public V1ListMeta Metadata { get; set; }
}

public class V1alpha1ApplicationSpec
{
    public V1alpha1ApplicationDestination Destination { get; set; }
    public List<V1alpha1ResourceIgnoreDifferences> IgnoreDifferences { get; set; }
    public List<V1alpha1Info> Info { get; set; }
    public string Project { get; set; }
    public long? RevisionHistoryLimit { get; set; }
    public V1alpha1ApplicationSource Source { get; set; }
    public List<V1alpha1ApplicationSource> Sources { get; set; }
    public V1alpha1SyncPolicy SyncPolicy { get; set; }
}

public class V1alpha1ApplicationStatus
{
    public List<V1alpha1ApplicationCondition> Conditions { get; set; }
    public string ControllerNamespace { get; set; }
    public V1alpha1HealthStatus Health { get; set; }
    public List<V1alpha1RevisionHistory> History { get; set; }
    public DateTime ObservedAt { get; set; }
    public V1alpha1OperationState OperationState { get; set; }
    public DateTime ReconciledAt { get; set; }
    public string ResourceHealthSource { get; set; }
    public List<V1alpha1ResourceStatus> Resources { get; set; }
    public string SourceType { get; set; }
    public List<string> SourceTypes { get; set; }
    public V1alpha1ApplicationSummary Summary { get; set; }
    public V1alpha1SyncStatus Sync { get; set; }
}

/// <summary>
/// ApplicationCondition contains details about an application condition, which is usually an error or warning.
/// </summary>
public class V1alpha1ApplicationCondition
{
    public DateTime LastTransitionTime { get; set; }

    /// <summary>
    /// Message contains human-readable message indicating details about the condition.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Type is an application condition type.
    /// </summary>
    public string Type { get; set; }
}

/// <summary>
/// ApplicationSummary contains information about URLs and container images used by an application.
/// </summary>
public class V1alpha1ApplicationSummary
{
    /// <summary>
    /// ExternalURLs holds all external URLs of application child resources.
    /// </summary>
    public List<string> ExternalURLs { get; set; }

    /// <summary>
    /// Images holds all images of application child resources.
    /// </summary>
    public List<string> Images { get; set; }
}

public class V1alpha1ApplicationDestination
{
    public string Name { get; set; }
    public string Namespace { get; set; }
    public string Server { get; set; }
}