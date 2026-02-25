using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Hosting.Api.Errors;

public class TokenExpiredError() : UnauthorizedError("Token is expired! Login again to retrive a new token.")
{
    [JsonPropertyName("expired_at")] public DateTime ExpiredAt { get; set; }
}