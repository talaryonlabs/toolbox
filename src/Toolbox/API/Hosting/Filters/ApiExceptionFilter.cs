using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Talaryon.Toolbox.API.Hosting.Filters;

public sealed class ApiExceptionFilter : ExceptionFilterAttribute
{
    private readonly ApiMediaType _mediaType;

    public ApiExceptionFilter(ApiMediaType mediaType)
    {
        _mediaType = mediaType;
    }
        
    public override void OnException(ExceptionContext context)
    {
        var apiError = context.Exception is ApiError error
            ? error
            : new InternalServerError(context.Exception);
            
        context.Result = new ObjectResult(apiError)
        {
            StatusCode = apiError.Code
        };
        context.HttpContext.Response.StatusCode = apiError.Code;
        context.HttpContext.Response.ContentType = _mediaType.MediaType.Value;
    }
}