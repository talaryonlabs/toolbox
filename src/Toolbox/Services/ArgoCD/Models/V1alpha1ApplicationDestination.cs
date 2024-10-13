namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1ApplicationDestination
{
    public string Name { get; set; }
    public string Namespace { get; set; }
    public string Server { get; set; }
}