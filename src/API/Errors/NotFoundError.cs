using Microsoft.AspNetCore.Http;

namespace TalaryonLabs.Toolbox.API;

public class NotFoundError : ApiError
{
    public NotFoundError(string? message) 
        : base(StatusCodes.Status404NotFound, message)
    {
            
    }
}