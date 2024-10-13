using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Moq;
using Talaryon.Toolbox.Services.ArgoCD;
using Talaryon.Toolbox.Services.ArgoCD.Models;

public class ArgoCDTests
{
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly Mock<IOptions<ArgoCDOptions>> _mockOptions;
    private readonly ArgoCD _argoCd;

    public ArgoCDTests()
    {
        _mockHttpClient = new Mock<HttpClient>();
        _mockOptions = new Mock<IOptions<ArgoCDOptions>>();
        
        var directusOptions = new ArgoCDOptions
        {
            BaseUrl = "http://example.com",
            AccessToken = "sample-token"
        };
        
        _mockOptions.Setup(x => x.Value).Returns(directusOptions);

        _argoCd = new ArgoCD(_mockHttpClient.Object, _mockOptions.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenOptionsAccessorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ArgoCD(_mockHttpClient.Object, null));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenBaseUrlIsNull()
    {
        var optionsAccessor = new Mock<IOptions<ArgoCDOptions>>();
        optionsAccessor.Setup(x => x.Value).Returns(new ArgoCDOptions());

        Assert.Throws<ArgumentNullException>(() => new ArgoCD(_mockHttpClient.Object, optionsAccessor.Object));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenAccessTokenIsNull()
    {
        var optionsAccessor = new Mock<IOptions<ArgoCDOptions>>();
        optionsAccessor.Setup(x => x.Value).Returns(new ArgoCDOptions { BaseUrl = "http://example.com" });

        Assert.Throws<ArgumentNullException>(() => new ArgoCD(_mockHttpClient.Object, optionsAccessor.Object));
    }
}