using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api.Errors;

public class BadRequestError() : ApiError(StatusCodes.Status403Forbidden, "Forbidden.");