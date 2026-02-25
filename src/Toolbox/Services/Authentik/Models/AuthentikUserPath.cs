using System.Text.Json.Serialization;
using Talaryon.Toolbox.Api;

namespace Talaryon.Toolbox.Services.Authentik.Models;

[ApiEndpoint("/core/users/paths/", ApiEndpointType.Single)]
public class AuthentikUserPath : IApiResource
{
    [JsonPropertyName("paths")] public string[] Paths { get; set; } = [];
}