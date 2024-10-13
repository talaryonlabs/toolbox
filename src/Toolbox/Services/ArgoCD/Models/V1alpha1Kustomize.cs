using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1KustomizeGvk
{
    public string Group { get; set; }
    public string Kind { get; set; }
    public string Version { get; set; }
}

public class V1alpha1KustomizeOptions
{
    public string BinaryPath { get; set; }
    public string BuildOptions { get; set; }
}

public class V1alpha1KustomizePatch
{
    public Dictionary<string, bool> Options { get; set; }
    public string Patch { get; set; }
    public string Path { get; set; }
    public V1alpha1KustomizeSelector Target { get; set; }
}

public class V1alpha1KustomizeReplica
{
    public IntstrIntOrString Count { get; set; }
    public string Name { get; set; }
}

public class V1alpha1KustomizeResId
{
    public V1alpha1KustomizeGvk Gvk { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; }
}

public class V1alpha1KustomizeSelector
{
    public string AnnotationSelector { get; set; }
    public string LabelSelector { get; set; }
    public V1alpha1KustomizeResId ResId { get; set; }
}