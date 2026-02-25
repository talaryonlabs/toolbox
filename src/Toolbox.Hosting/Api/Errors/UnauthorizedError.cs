using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api;

[DataContract]
public class UnauthorizedError : ApiError
{
    public UnauthorizedError(string? message) 
        : base(StatusCodes.Status401Unauthorized, message)
    {
    }
}