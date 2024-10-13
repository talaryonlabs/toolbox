using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1SyncWindow
{
    public List<string> Applications { get; set; }
    public List<string> Clusters { get; set; }
    public string Duration { get; set; }
    public string Kind { get; set; }
    public bool? ManualSync { get; set; }
    public List<string> Namespaces { get; set; }
    public string Schedule { get; set; }
    public string TimeZone { get; set; }
}