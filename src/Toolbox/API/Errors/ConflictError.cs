﻿using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Talaryon.Toolbox.API;

[DataContract]
public class ConflictError : ApiError
{
    public ConflictError(string? message) 
        : base(StatusCodes.Status409Conflict, message)
    {
    }
}