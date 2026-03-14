using Microsoft.Extensions.Options;
using Moq;
using Talaryon.Toolbox.Services.ArgoCD;

namespace Talaryon.Toolbox.Tests.Services
{
    public class ArgoCDTests
    {
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<IOptions<ArgoCDOptions>> _mockOptions;
        private readonly ArgoCD _argoCd;

        public ArgoCDTests()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _mockOptions = new Mock<IOptions<ArgoCDOptions>>();
            
            var argoCDOptions = new ArgoCDOptions
            {
                BaseUrl = "http://example.com",
                AccessToken = "sample-token"
            };
            
            _mockOptions.Setup(x => x.Value).Returns(argoCDOptions);

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

            var exception = Assert.Throws<ArgumentNullException>(() => new ArgoCD(_mockHttpClient.Object, optionsAccessor.Object));
            Assert.Equal("BaseUrl", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenAccessTokenIsNull()
        {
            var optionsAccessor = new Mock<IOptions<ArgoCDOptions>>();
            optionsAccessor.Setup(x => x.Value).Returns(new ArgoCDOptions { BaseUrl = "http://example.com" });

            // Just verify that an ArgumentNullException is thrown
            Assert.Throws<ArgumentNullException>(() => new ArgoCD(_mockHttpClient.Object, optionsAccessor.Object));
        }

        [Fact]
        public void Constructor_ShouldInitializeWithValidOptions()
        {
            // Act & Assert - constructor doesn't throw
            Assert.NotNull(_argoCd);
        }
    }
}