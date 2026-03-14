using System.Text.Json.Serialization;
using Talaryon.Toolbox.Hosting.Api;

namespace Talaryon.Toolbox.Tests.Hosting;

public class ApiControllerTests
{
    [Fact]
    public void ApiRequest_OperatorPlus_ShouldMergeProperties()
    {
        // Arrange
        var request = new ApiRequest<TestItem>
        {
            Items = new Dictionary<string, object?>
            {
                { "Name", "UpdatedName" },
                { "Value", 100 }
            }
        };
        
        var originalItem = new TestItem { Name = "OriginalName", Value = 50 };
        
        // Act
        var result = request + originalItem;
        
        // Assert
        Assert.Equal("UpdatedName", result.Name);
        Assert.Equal(100, result.Value);
    }
    
    [Fact]
    public void ApiList_ShouldInitializeCorrectly()
    {
        // Arrange
        var items = new List<TestItem>
        {
            new() { Name = "Item1", Value = 1 },
            new() { Name = "Item2", Value = 2 }
        };
        
        // Act
        var apiList = new ApiList<TestItem>
        {
            Items = items,
            NextCursor = "cursor123",
            TotalCount = 2
        };
        
        // Assert
        Assert.Equal(2, apiList.TotalCount);
        Assert.Equal("cursor123", apiList.NextCursor);
        Assert.Equal(2, apiList.Items.Count());
    }
    
    [Fact]
    public void ApiListArgs_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var args = new ApiListArgs();
        
        // Assert
        Assert.Equal(string.Empty, args.Cursor);
        Assert.Equal(0, args.Skip);
        Assert.Equal(0, args.Limit);
    }
    
    private class TestItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
    
    private class TestController : ApiController<TestItem, TestListArgs>
    {
        protected override Task<ApiList<TestItem>> List(TestListArgs args, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ApiList<TestItem>());
        }
        
        protected override Task<TestItem> Get(string itemId, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestItem());
        }
        
        protected override Task<TestItem> Create(ApiRequest<TestItem> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestItem());
        }
        
        protected override Task<TestItem> Update(string itemId, ApiRequest<TestItem> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestItem());
        }
        
        protected override Task<TestItem> Delete(string itemId, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestItem());
        }
    }
    
    private class TestListArgs : ApiListArgs
    {
        // Test-specific arguments
    }
}