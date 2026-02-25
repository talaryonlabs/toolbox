using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public class AuthenticationError() : ApiError(StatusCodes.Status401Unauthorized,
    "Authentication failed. Username and Password correct?");