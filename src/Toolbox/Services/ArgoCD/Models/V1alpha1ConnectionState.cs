using System;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1ConnectionState
{
    public DateTime AttemptedAt { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
}