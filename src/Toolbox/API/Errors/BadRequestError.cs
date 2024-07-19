using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.API;

[DataContract]
public class BadRequestError : ApiError
{
    public BadRequestError() : base(StatusCodes.Status403Forbidden, "Forbidden.")
    {
    }
}