using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1ApplicationSource
{
public string Chart { get; set; }
public V1alpha1ApplicationSourceDirectory Directory { get; set; }
public V1alpha1ApplicationSourceHelm Helm { get; set; }
public V1alpha1ApplicationSourceKustomize Kustomize { get; set; }
public string Path { get; set; }
public V1alpha1ApplicationSourcePlugin Plugin { get; set; }
public string Ref { get; set; }
public string RepoURL { get; set; }
public string TargetRevision { get; set; }
}

public class V1alpha1ApplicationSourceDirectory
{
    public string Exclude { get; set; }
    public string Include { get; set; }
    public V1alpha1ApplicationSourceJsonnet Jsonnet { get; set; }
    public bool Recurse { get; set; }
}

public class V1alpha1ApplicationSourceHelm
{
    public List<V1alpha1HelmFileParameter> FileParameters { get; set; }
    public bool IgnoreMissingValueFiles { get; set; }
    public List<V1alpha1HelmParameter> Parameters { get; set; }
    public bool PassCredentials { get; set; }
    public string ReleaseName { get; set; }
    public bool SkipCrds { get; set; }
    public List<string> ValueFiles { get; set; }
    public string Values { get; set; }
    public RuntimeRawExtension ValuesObject { get; set; }
    public string Version { get; set; }
}

public class V1alpha1ApplicationSourceJsonnet
{
    public List<V1alpha1JsonnetVar> ExtVars { get; set; }
    public List<string> Libs { get; set; }
    public List<V1alpha1JsonnetVar> Tlas { get; set; }
}

public class V1alpha1ApplicationSourceKustomize
{
    public Dictionary<string, string> CommonAnnotations { get; set; }
    public bool CommonAnnotationsEnvsubst { get; set; }
    public Dictionary<string, string> CommonLabels { get; set; }
    public List<string> Components { get; set; }
    public bool ForceCommonAnnotations { get; set; }
    public bool ForceCommonLabels { get; set; }
    public List<string> Images { get; set; }
    public bool LabelWithoutSelector { get; set; }
    public string NamePrefix { get; set; }
    public string NameSuffix { get; set; }
    public string Namespace { get; set; }
    public List<V1alpha1KustomizePatch> Patches { get; set; }
    public List<V1alpha1KustomizeReplica> Replicas { get; set; }
    public string Version { get; set; }
}

public class V1alpha1ApplicationSourcePlugin
{
    public List<ApplicationV1alpha1EnvEntry> Env { get; set; }
    public string Name { get; set; }
    public List<V1alpha1ApplicationSourcePluginParameter> Parameters { get; set; }
}

public class V1alpha1ApplicationSourcePluginParameter
{
    public List<string> Array { get; set; }
    public Dictionary<string, string> Map { get; set; }
    public string Name { get; set; }
    public string String { get; set; }
}