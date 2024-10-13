using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1AppProjectList
{
    public List<V1alpha1AppProject> Items { get; set; }
    public V1ListMeta Metadata { get; set; }
}