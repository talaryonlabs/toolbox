using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api;

[DataContract]
public class ConflictError : ApiError
{
    public ConflictError(string? message) 
        : base(StatusCodes.Status409Conflict, message)
    {
    }
}