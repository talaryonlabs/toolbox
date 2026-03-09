using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public sealed class BadRequestError : ApiError
{
    public BadRequestError(string? message)
        : base(StatusCodes.Status400BadRequest, message)
    {
    }

    public BadRequestError(Exception exception)
        : base(StatusCodes.Status400BadRequest, exception)
    {
    }
}