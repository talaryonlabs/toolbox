using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// ComparedTo contains application source and target which was used for resources comparison.
/// </summary>
public class V1alpha1ComparedTo
{
    public V1alpha1ApplicationDestination Destination { get; set; }

    /// <summary>
    /// IgnoreDifferences is a reference to the application's ignored differences used for comparison.
    /// </summary>
    public List<V1alpha1ResourceIgnoreDifferences> IgnoreDifferences { get; set; }

    public V1alpha1ApplicationSource Source { get; set; }

    /// <summary>
    /// Sources is a reference to the application's multiple sources used for comparison.
    /// </summary>
    public List<V1alpha1ApplicationSource> Sources { get; set; }
}