using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Talaryon.Toolbox.Extensions;
using Talaryon.Toolbox.Hosting.Api.Errors;

namespace Talaryon.Toolbox.Hosting.Api.Filters;

public sealed class ApiExceptionFilter(ApiMediaType mediaType) : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = context.Exception as ApiError ?? new InternalServerError(context.Exception);
        var data = apiError
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(v => v.HasCustomAttribute<JsonPropertyNameAttribute>())
            .ToDictionary(
                v => v.GetCustomAttribute<JsonPropertyNameAttribute>(true)!.Name, 
                v => v.GetValue(apiError)?.ToString() ?? ""
            );
            
        context.Result = new ObjectResult(data)
        {
            StatusCode = apiError.Code
        };
        context.HttpContext.Response.StatusCode = apiError.Code;
        context.HttpContext.Response.ContentType = mediaType.MediaType.Value;
    }
}