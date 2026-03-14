using Talaryon.Toolbox.Api;

namespace Talaryon.Toolbox.Tests.Api;

public class ApiRequestParamsTests
{
    [Fact]
    public void ApiRequestParams_ShouldInitializeEmpty()
    {
        // Arrange & Act
        var paramsObj = new ApiRequestParams();
        
        // Assert
        Assert.Empty(paramsObj.ToDictionary());
        Assert.Equal("", paramsObj.ToQueryString().ToString());
    }
    
    [Fact]
    public void ApiRequestParams_Set_ShouldAddParameter()
    {
        // Arrange
        var paramsObj = new ApiRequestParams();
        
        // Act
        paramsObj.Set("key1", "value1");
        
        // Assert
        var dict = paramsObj.ToDictionary();
        Assert.Single(dict);
        Assert.Equal("value1", dict["key1"]);
    }
    
    [Fact]
    public void ApiRequestParams_Set_ShouldUpdateExistingParameter()
    {
        // Arrange
        var paramsObj = new ApiRequestParams();
        paramsObj.Set("key1", "value1");
        
        // Act
        paramsObj.Set("key1", "updated_value");
        
        // Assert
        var dict = paramsObj.ToDictionary();
        Assert.Single(dict);
        Assert.Equal("updated_value", dict["key1"]);
    }
    
    [Fact]
    public void ApiRequestParams_ToQueryString_ShouldConvertToQueryString()
    {
        // Arrange
        var paramsObj = new ApiRequestParams();
        paramsObj.Set("key1", "value1");
        paramsObj.Set("key2", "value2");
        
        // Act
        var queryString = paramsObj.ToQueryString().ToString();
        
        // Assert
        Assert.Contains("key1=value1", queryString);
        Assert.Contains("key2=value2", queryString);
        Assert.Contains("&", queryString);
    }
    
    [Fact]
    public void ApiRequestParams_ToQueryString_ShouldHandleEmptyValues()
    {
        // Arrange
        var paramsObj = new ApiRequestParams();
        paramsObj.Set("key1", "value1");
        paramsObj.Set("key2", "");
        
        // Act
        var queryString = paramsObj.ToQueryString().ToString();
        
        // Assert
        Assert.Contains("key1=value1", queryString);
        Assert.Contains("key2=", queryString);
    }
}