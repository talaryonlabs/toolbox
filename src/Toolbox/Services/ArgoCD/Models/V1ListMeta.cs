namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1ListMeta
{
    public string Continue { get; set; }
    public long? RemainingItemCount { get; set; }
    public string ResourceVersion { get; set; }
    public string SelfLink { get; set; }
}