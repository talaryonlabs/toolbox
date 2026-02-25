using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api.Errors;

public class NotFoundError(string? message) : ApiError(StatusCodes.Status404NotFound, message);