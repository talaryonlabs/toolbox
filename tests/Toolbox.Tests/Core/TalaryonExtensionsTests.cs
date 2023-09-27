using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TalaryonLabs.Toolbox.Tests
{
    public class TalaryonExtensionsTests
    {
        [Fact]
        public void AddSingleton_Should_AddSingletonService()
        {
            // Arrange
            var services = new ServiceCollection();
            var options = new TestOptions();

            // Act
            services.AddSingleton(options);
            services.AddSingleton<ITestService, TestService>();
            services.Configure<TestOptions>(o => o = options);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();
            Assert.NotNull(service);
            Assert.IsType<TestService>(service);
        }

        [Fact]
        public void AddScoped_Should_AddScopedService()
        {
            // Arrange
            var services = new ServiceCollection();
            var options = new TestOptions();

            // Act
            services.AddSingleton(options);
            services.AddScoped<ITestService, TestService>();
            services.Configure<TestOptions>(o => o = options);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();
            Assert.NotNull(service);
            Assert.IsType<TestService>(service);
        }

        [Fact]
        public void AddTransient_Should_AddTransientService()
        {
            // Arrange
            var services = new ServiceCollection();
            var options = new TestOptions();

            // Act
            services.AddSingleton(options);
            services.AddTransient<ITestService, TestService>();
            services.Configure<TestOptions>(o => o = options);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();
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

    public class TestConfig : IOptions<TestConfig>
    {
        public TestConfig Value { get; set; } = null!;
    }

    public class TestOptions : IOptions<TestOptions>
    {
        public TestOptions Value => this;

        public void Configure(TestOptions options)
        {
        }
    }
}