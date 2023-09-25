using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace TalaryonLabs.Toolbox.API;

[DataContract]
public class NotImplementedError : ApiError
{
    public NotImplementedError() 
        : base(StatusCodes.Status501NotImplemented, "Method not implemented.")
    {
    }
}