﻿using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1ObjectMeta
{
    public Dictionary<string, string> Annotations { get; set; }
    public V1Time CreationTimestamp { get; set; }
    public long? DeletionGracePeriodSeconds { get; set; }
    public V1Time DeletionTimestamp { get; set; }
    public List<string> Finalizers { get; set; }
    public string GenerateName { get; set; }
    public long? Generation { get; set; }
    public Dictionary<string, string> Labels { get; set; }
    public List<V1ManagedFieldsEntry> ManagedFields { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; }
    public List<V1OwnerReference> OwnerReferences { get; set; }
    public string ResourceVersion { get; set; }
    public string SelfLink { get; set; }
    public string Uid { get; set; }
}