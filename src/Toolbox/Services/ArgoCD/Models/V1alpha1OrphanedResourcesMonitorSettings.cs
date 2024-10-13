using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1OrphanedResourcesMonitorSettings
{
    public List<V1alpha1OrphanedResourceKey> Ignore { get; set; }
    public bool? Warn { get; set; }
}