using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Talaryon.Toolbox.Hosting;
using Talaryon.Toolbox.Hosting.Api;

namespace Talaryon.Toolbox.Tests.Hosting;

public class HostingExtensionsTests
{
    [Fact]
    public void ToQueryString_ShouldConvertObjectToQueryString()
    {
        // Arrange
        var testObject = new TestQueryObject { Name = "test", Value = 42 };
        
        // Act
        var result = HostingExtensions.ToQueryString(testObject);
        
        // Assert
        Assert.Contains("name=test", result);
        Assert.Contains("value=42", result);
    }
    
    [Fact]
    public void AddApiComponents_ShouldConfigureServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var options = new ApiHostingOptions
        {
            RateLimit = 100,
            QueueLimit = 2,
            IsTokenAuthenticationEnabled = true,
            AccessTokens = new List<string> { "test-token" }
        };
        
        // Act
        services.AddApiComponents(options);
        
        // Assert
        // Basic verification that services were added
        Assert.True(services.Count > 0);
    }
    
    [Fact]
    public void BuildAsApi_ShouldConfigureMiddleware()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var options = new ApiHostingOptions
        {
            IsTokenAuthenticationEnabled = true,
            AccessTokens = new List<string> { "test-token" }
        };
        
        builder.Services.AddApiComponents(options);
        
        // Act
        var app = builder.BuildAsApi(options);
        
        // Assert
        // Verify middleware pipeline is configured
        Assert.NotNull(app);
    }
    
    private class TestQueryObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}