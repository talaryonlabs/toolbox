using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace TalaryonLabs.Toolbox.API;

[DataContract]
public class ForbiddenError : ApiError
{
    public ForbiddenError() 
        : base(StatusCodes.Status403Forbidden, "Forbidden.")
    {
    }
}