using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api.Errors;

public class ForbiddenError() : ApiError(StatusCodes.Status403Forbidden, "Forbidden.");