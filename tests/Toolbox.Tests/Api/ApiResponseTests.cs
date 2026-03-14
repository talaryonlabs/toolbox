using Talaryon.Toolbox.Api;

namespace Talaryon.Toolbox.Tests.Api;

public class ApiResponseTests
{
    [Fact]
    public void ApiResponse_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var response = new ApiResponse<string>();
        
        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(0, response.StatusCode);
        Assert.Null(response.Error);
        Assert.Null(response.Data);
    }
    
    [Fact]
    public void ApiResponse_ShouldHandleSuccessfulResponse()
    {
        // Arrange & Act
        var response = new ApiResponse<string>
        {
            IsSuccessful = true,
            StatusCode = 200,
            Data = "test data"
        };
        
        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("test data", response.Data);
        Assert.Null(response.Error);
    }
    
    [Fact]
    public void ApiResponse_ShouldHandleErrorResponse()
    {
        // Arrange
        var error = new ApiError(404, "Not found");
        
        // Act
        var response = new ApiResponse<string>
        {
            IsSuccessful = false,
            StatusCode = 404,
            Error = error
        };
        
        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(404, response.StatusCode);
        Assert.Equal(error, response.Error);
        Assert.Null(response.Data);
    }
}