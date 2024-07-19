using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.API;

[DataContract]
public class AuthenticationError : ApiError
{
    public AuthenticationError() : base(StatusCodes.Status401Unauthorized, "Authentication failed. Username and Password correct?")
    {
    }
}