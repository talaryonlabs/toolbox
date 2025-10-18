using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.API.Hosting;

public class NotFoundError : ApiError
{
    public NotFoundError(string? message) 
        : base(StatusCodes.Status404NotFound, message)
    {
            
    }
}