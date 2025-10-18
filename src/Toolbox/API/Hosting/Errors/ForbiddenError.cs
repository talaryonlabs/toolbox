using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.API.Hosting;

[DataContract]
public class ForbiddenError : ApiError
{
    public ForbiddenError() 
        : base(StatusCodes.Status403Forbidden, "Forbidden.")
    {
    }
}