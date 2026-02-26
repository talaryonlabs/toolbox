namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1SyncWindow
{
    public List<string> Applications { get; set; } = new();
    public List<string> Clusters { get; set; } = new();
    public string? Duration { get; set; }
    public string? Kind { get; set; }
    public bool ManualSync { get; set; }
    public List<string> Namespaces { get; set; } = new();
    public string? Schedule { get; set; }
    public string? TimeZone { get; set; }
}