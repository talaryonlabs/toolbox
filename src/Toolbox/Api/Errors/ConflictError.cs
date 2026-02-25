using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public class ConflictError(string? message) : ApiError(StatusCodes.Status409Conflict, message);