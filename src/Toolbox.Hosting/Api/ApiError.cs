using System.Text.Json.Serialization;

namespace Talaryon.Toolbox.Hosting.Api;

public class ApiError() : Exception
{
    [JsonPropertyName("code")] public int Code { get; set; }
    [JsonPropertyName("request_id")] public string? RequestId { get; set; }
    [JsonPropertyName("message")] public new string? Message { get; set; }
    [JsonPropertyName("documentation_url")] public string DocumentationUrl { get; set; } = "https://github.com/talaryonstudios/toolbox";
    [JsonPropertyName("stack_trace")] private new string? StackTrace { get; set; }

    public ApiError(int code, string? message)
        : this()
    {
        Code = code;
        Message = message;
    }

    public ApiError(int code, Exception e)
        : this(code, e.Message)
    {
        StackTrace = e.StackTrace;
    }
}