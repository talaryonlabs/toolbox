using Talaryon.Toolbox.Api;

namespace Talaryon.Toolbox.Tests.Api;

public class ApiEndpointTests
{
    [Fact]
    public void GetEndpoint_ShouldReturnUrlForManyEndpoint()
    {
        // Arrange & Act
        var endpoint = ApiEndpoint.GetEndpoint<TestResource>();
        
        // Assert
        Assert.Equal("/api/resources", endpoint);
    }
    
    [Fact]
    public void GetEndpoint_ShouldReturnUrlForSingleEndpoint()
    {
        // Arrange & Act
        var endpoint = ApiEndpoint.GetEndpoint<TestResource>(ApiEndpointType.Single);
        
        // Assert
        Assert.Equal("/api/resources/{id}", endpoint);
    }
    
    [Fact]
    public void GetEndpoint_ShouldReturnNullForNonExistentEndpointType()
    {
        // Arrange & Act
        var endpoint = ApiEndpoint.GetEndpoint<TestResource>(ApiEndpointType.Create);
        
        // Assert
        Assert.Null(endpoint);
    }
    
    [Fact]
    public void GetEndpoint_ShouldReturnNullForResourceWithoutAttribute()
    {
        // Arrange & Act
        var endpoint = ApiEndpoint.GetEndpoint<ResourceWithoutAttribute>();
        
        // Assert
        Assert.Null(endpoint);
    }
    
    [ApiEndpoint("/api/resources", ApiEndpointType.Many)]
    [ApiEndpoint("/api/resources/{id}", ApiEndpointType.Single)]
    private class TestResource
    {
    }
    
    private class ResourceWithoutAttribute
    {
    }
}