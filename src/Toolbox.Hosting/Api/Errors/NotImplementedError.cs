using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.Hosting.Api;

[DataContract]
public class NotImplementedError : ApiError
{
    public NotImplementedError() 
        : base(StatusCodes.Status501NotImplemented, "Method not implemented.")
    {
    }
}