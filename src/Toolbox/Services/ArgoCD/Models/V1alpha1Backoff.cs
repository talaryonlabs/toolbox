namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1Backoff
{
    public string Duration { get; set; }
    public long Factor { get; set; }
    public string MaxDuration { get; set; }
}