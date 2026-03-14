using System.Text.Json.Serialization;
using Talaryon.Toolbox.Api;
using Talaryon.Toolbox.Api.Errors;

namespace Talaryon.Toolbox.Tests.Api;

public class ApiErrorTests
{
    [Fact]
    public void ApiError_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var error = new ApiError();
        
        // Assert
        Assert.Equal(0, error.Code);
        Assert.Null(error.RequestId);
        Assert.Null(error.Message);
        Assert.Equal("https://github.com/talaryonstudios/toolbox", error.DocumentationUrl);
        Assert.Null(error.StackTrace);
    }
    
    [Fact]
    public void ApiError_ShouldInitializeWithCodeAndMessage()
    {
        // Arrange & Act
        var error = new ApiError(404, "Resource not found");
        
        // Assert
        Assert.Equal(404, error.Code);
        Assert.Equal("Resource not found", error.Message);
    }
    
    [Fact]
    public void ApiError_ShouldInitializeWithCodeAndException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        
        // Act
        var error = new ApiError(500, exception);
        
        // Assert
        Assert.Equal(500, error.Code);
        Assert.Equal("Test exception", error.Message);
        Assert.Equal(exception.StackTrace, error.StackTrace);
    }
    
    [Fact]
    public void BadRequestError_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var error = new BadRequestError("Invalid request");
        
        // Assert
        Assert.Equal(400, error.Code);
        Assert.Equal("Invalid request", error.Message);
    }
    
    [Fact]
    public void NotFoundError_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var error = new NotFoundError("Resource not found");
        
        // Assert
        Assert.Equal(404, error.Code);
        Assert.Equal("Resource not found", error.Message);
    }
    
    [Fact]
    public void ApiError_ShouldHaveJsonPropertyNames()
    {
        // Arrange
        var error = new ApiError(500, "Internal server error");
        
        // Act
        var properties = typeof(ApiError).GetProperties();
        var codeAttr = properties.First(p => p.Name == "Code").GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute;
        var messageAttr = properties.First(p => p.Name == "Message").GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute;
        var requestIdAttr = properties.First(p => p.Name == "RequestId").GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute;
        var documentationUrlAttr = properties.First(p => p.Name == "DocumentationUrl").GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault() as JsonPropertyNameAttribute;
        
        // Assert
        Assert.Equal("code", codeAttr?.Name);
        Assert.Equal("message", messageAttr?.Name);
        Assert.Equal("request_id", requestIdAttr?.Name);
        Assert.Equal("documentation_url", documentationUrlAttr?.Name);
    }
}