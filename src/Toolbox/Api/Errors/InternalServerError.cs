using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public sealed class InternalServerError : ApiError
{
    public InternalServerError()
        : base(StatusCodes.Status500InternalServerError, "Unknown Error")
    {
    }

    public InternalServerError(string? message)
        : base(StatusCodes.Status500InternalServerError, message)
    {
    }

    public InternalServerError(Exception exception)
        : base(StatusCodes.Status500InternalServerError, exception)
    {
    }
}