using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api;

public class NotFoundError : ApiError
{
    public NotFoundError(string? message) 
        : base(StatusCodes.Status404NotFound, message)
    {
            
    }
}