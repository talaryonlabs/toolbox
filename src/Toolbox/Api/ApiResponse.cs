namespace Talaryon.Toolbox.Api;

public class ApiResponse<T>
{
    public bool IsSuccessful { get; set; }
    public int StatusCode { get; set; }
    public ApiError? Error { get; set; }
    public T? Data { get; set; }
}