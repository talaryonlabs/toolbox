namespace Talaryon.Toolbox.Services.ArgoCD.Models;

/// <summary>
/// ResourceIgnoreDifferences contains resource filter and a list of JSON paths which should be ignored during comparison with live state.
/// </summary>
public class V1alpha1ResourceIgnoreDifferences
{
    public string Group { get; set; }

    public List<string> JqPathExpressions { get; set; }

    public List<string> JsonPointers { get; set; }

    public string Kind { get; set; }

    /// <summary>
    /// ManagedFieldsManagers is a list of trusted managers. Fields mutated by those managers will take precedence over the
    /// desired state defined in the SCM and won't be displayed in diffs.
    /// </summary>
    public List<string> ManagedFieldsManagers { get; set; }

    public string Name { get; set; }

    public string Namespace { get; set; }
}