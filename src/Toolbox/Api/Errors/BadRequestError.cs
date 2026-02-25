using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public class BadRequestError() : ApiError(StatusCodes.Status403Forbidden, "Forbidden.");