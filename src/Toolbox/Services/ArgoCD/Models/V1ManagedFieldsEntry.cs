namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1ManagedFieldsEntry
{
    public string ApiVersion { get; set; }
    public string FieldsType { get; set; }
    public V1FieldsV1 FieldsV1 { get; set; }
    public string Manager { get; set; }
    public string Operation { get; set; }
    public string Subresource { get; set; }
    public V1Time Time { get; set; }
}