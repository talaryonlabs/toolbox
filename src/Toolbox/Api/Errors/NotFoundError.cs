using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public class NotFoundError(string? message) : ApiError(StatusCodes.Status404NotFound, message);