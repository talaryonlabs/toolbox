namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1JWTToken
{
    public long Exp { get; set; } 
    public long Iat { get; set; } 
    public string? Id { get; set; } 
}

public class V1alpha1JWTTokens
{
    public List<V1alpha1JWTToken> Items { get; set; } = new();
}