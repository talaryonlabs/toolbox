using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api;

[DataContract]
public class ForbiddenError : ApiError
{
    public ForbiddenError() 
        : base(StatusCodes.Status403Forbidden, "Forbidden.")
    {
    }
}