using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Talaryon.Toolbox.Api.Errors;
using Talaryon.Toolbox.Hosting.Api;
using Talaryon.Toolbox.Hosting.Api.Filters;

namespace Talaryon.Toolbox.Tests.Hosting;

public class ApiExceptionFilterTests
{
    [Fact]
    public void OnException_ShouldHandleApiError()
    {
        // Arrange
        var mediaType = new ApiMediaType();
        var filter = new ApiExceptionFilter(mediaType);
        var context = CreateExceptionContext(new BadRequestError("Test error"));
        
        // Act
        filter.OnException(context);
        
        // Assert
        Assert.IsType<ObjectResult>(context.Result);
        var result = context.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status400BadRequest, result?.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, context.HttpContext.Response.StatusCode);
    }
    
    [Fact]
    public void OnException_ShouldHandleGenericException()
    {
        // Arrange
        var mediaType = new ApiMediaType();
        var filter = new ApiExceptionFilter(mediaType);
        var context = CreateExceptionContext(new Exception("Generic error"));
        
        // Act
        filter.OnException(context);
        
        // Assert
        Assert.IsType<ObjectResult>(context.Result);
        var result = context.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, result?.StatusCode);
    }
    
    [Fact]
    public void OnException_ShouldSetContentType()
    {
        // Arrange
        var mediaType = new ApiMediaType();
        var filter = new ApiExceptionFilter(mediaType);
        var context = CreateExceptionContext(new Exception("Test error"));
        
        // Act
        filter.OnException(context);
        
        // Assert
        Assert.Equal(mediaType.MediaType.Value, context.HttpContext.Response.ContentType);
    }
    
    private static ExceptionContext CreateExceptionContext(Exception exception)
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
        
        return context;
    }
}