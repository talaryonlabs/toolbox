using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Tests
{
    public class TalaryonExtensionsTests
    {
        [Fact]
        public void AddConfig_Should_AddConfigurationToServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new TestConfig();

            // Act
            services.AddConfig(config);
            var serviceProvider = services.BuildServiceProvider();
            var retrievedConfig = serviceProvider.GetService<IOptions<TestConfig>>();

            // Assert
            Assert.NotNull(retrievedConfig);
            Assert.Same(config, retrievedConfig.Value);
        }

        [Fact]
        public void AddSingleton_Should_AddSingletonServiceWithOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSingleton<ITestService, TestService, TestOptions>(options =>
            {
                options.TestValue = "configured";
            });
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType<TestService>(service);
        }

        [Fact]
        public void AddScoped_Should_AddScopedServiceWithOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddScoped<ITestService, TestService, TestOptions>(options =>
            {
                options.TestValue = "configured";
            });
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType<TestService>(service);
        }

        [Fact]
        public void AddTransient_Should_AddTransientServiceWithOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddTransient<ITestService, TestService, TestOptions>(options =>
            {
                options.TestValue = "configured";
            });
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType<TestService>(service);
        }
    }

    public interface ITestService
    {
    }

    public class TestService : ITestService
    {
    }

    public class TestOptions : IOptions<TestOptions>
    {
        public string TestValue { get; set; } = string.Empty;
        public TestOptions Value => this;
    }

    public class TestConfig : IOptions<TestConfig>
    {
        public TestConfig Value => this;
    }
}