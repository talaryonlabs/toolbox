using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Services.Hub;

[HubEndpoint("/hub/api/rest/users", ResponseType = typeof(HubUserList), Type =  HubEndpointType.List | HubEndpointType.Create)]
[HubEndpoint("/hub/api/rest/users/.id", HubEndpointType.Get)]
[HubAdditionalFields("details(email)")]
public class HubUser : IHubResource
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("login")] public string? Login { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("userType")] public HubUserType? UserType { get; set; }
    [JsonPropertyName("details")] public HubUserDetails[]? Details { get; set; }
}

public class HubUserType
{
    [JsonPropertyName("id")] public string? Id { get; set; }
}

public class HubUserDetails
{
    [JsonPropertyName("type")] public string? Type { get; set; } = "LoginuserdetailsJSON";
    [JsonPropertyName("email")] public HubUserDetailsEmail? Email { get; set; }
}

public class HubUserDetailsEmail
{
    [JsonPropertyName("type")] public string? Type { get; set; } = "EmailJSON";
    [JsonPropertyName("verified")] public bool Verified { get; set; }
    [JsonPropertyName("email")] public string? EMail { get; set; }
}

public class HubUserList : IHubResponseType
{
    [JsonPropertyName("skip")] public int Skip { get; set; }
    [JsonPropertyName("top")] public int Top { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("users")] public List<HubUser>? List { get; set; }
}

public class HubUserParams : HubParams
{
    public HubUserParams Name(string name) => (HubUserParams)Set("name", name);
    public HubUserParams Login(string login) => (HubUserParams)Set("login", login);
    public HubUserParams Email(string email) => (HubUserParams)Set("details", new HubUserDetails[]
    {
        new()
        {
            Email = new HubUserDetailsEmail(){EMail = email, Verified = true}
        }
    });

    public HubUserParams UserType(string userType) =>
        (HubUserParams)Set("userType", new HubUserType() { Id = userType });
}