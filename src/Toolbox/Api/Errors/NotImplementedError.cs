using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Api.Errors;

public class NotImplementedError() : ApiError(StatusCodes.Status501NotImplemented, "Method not implemented.");