using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api.Errors;

public class UnauthorizedError(string? message) : ApiError(StatusCodes.Status401Unauthorized, message);