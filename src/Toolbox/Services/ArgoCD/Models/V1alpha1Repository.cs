using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.ArgoCD.Models;

public class V1alpha1Repository
{
    public V1alpha1ConnectionState ConnectionState { get; set; }
    public bool? EnableLFS { get; set; }
    public bool? EnableOCI { get; set; }
    public bool? ForceHttpBasicAuth { get; set; }
    public string GcpServiceAccountKey { get; set; }
    public string GithubAppEnterpriseBaseUrl { get; set; }
    public long? GithubAppID { get; set; }
    public long? GithubAppInstallationID { get; set; }
    public string GithubAppPrivateKey { get; set; }
    public bool? InheritedCreds { get; set; }
    public bool? Insecure { get; set; }
    public bool? InsecureIgnoreHostKey { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Project { get; set; }
    public string Proxy { get; set; }
    public string Repo { get; set; }
    public string SshPrivateKey { get; set; }
    public string TlsClientCertData { get; set; }
    public string TlsClientCertKey { get; set; }
    public string Type { get; set; }
    public string Username { get; set; }
}

public class V1alpha1RepositoryList
{
    public List<V1alpha1Repository> Items { get; set; }
    public V1ListMeta Metadata { get; set; }
}

public class V1alpha1RepositoryCertificate
{
    public byte[] CertData { get; set; }
    public string CertInfo { get; set; }
    public string CertSubType { get; set; }
    public string CertType { get; set; }
    public string ServerName { get; set; }
}

public class V1alpha1RepositoryCertificateList
{
    public List<V1alpha1RepositoryCertificate> Items { get; set; }
    public V1ListMeta Metadata { get; set; }
}