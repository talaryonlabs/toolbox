using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api;

[DataContract]
public class BadRequestError : ApiError
{
    public BadRequestError() : base(StatusCodes.Status403Forbidden, "Forbidden.")
    {
    }
}