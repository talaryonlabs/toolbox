using System.Text.RegularExpressions;

namespace Talaryon.Toolbox.Tests
{
    public class TalaryonHelperTests
    {
        [Fact]
        public void SerializeObject_Should_SerializeObject()
        {
            // Arrange
            var obj = new TestObject { Id = 1, Name = "Test" };

            // Act
            var result = TalaryonHelper.SerializeObject(obj);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("\"Id\":1", System.Text.Encoding.UTF8.GetString(result));
            Assert.Contains("\"Name\":\"Test\"", System.Text.Encoding.UTF8.GetString(result));
        }

        [Fact]
        public void DeserializeObject_Should_DeserializeObject()
        {
            // Arrange
            var obj = new TestObject { Id = 1, Name = "Test" };
            var data = TalaryonHelper.SerializeObject(obj);

            // Act
            var result = TalaryonHelper.DeserializeObject<TestObject>(data);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(obj.Id, result.Id);
            Assert.Equal(obj.Name, result.Name);
        }

        [Fact]
        public void UUID_Should_ReturnValidUUID()
        {
            // Act
            var result = TalaryonHelper.UUID();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<string>(result);
            // Verify UUID format (8-4-4-4-12 hex digits)
            Assert.Matches("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", result);
        }

        [Theory]
        [InlineData("1K", 1024)]
        [InlineData("1KB", 1024)]
        [InlineData("1M", 1048576)]
        [InlineData("1MB", 1048576)]
        [InlineData("1G", 1073741824)]
        [InlineData("1GB", 1073741824)]
        [InlineData("1T", 1099511627776)]
        [InlineData("1TB", 1099511627776)]
        [InlineData("1P", 1125899906842624)]
        [InlineData("1PB", 1125899906842624)]
        [InlineData("1E", 1152921504606846976)]
        [InlineData("1EB", 1152921504606846976)]
        [InlineData("1024", 1024)]
        public void ParseNamedSize_Should_ParseNamedSize(string namedSize, ulong expectedSize)
        {
            // Act
            var result = TalaryonHelper.ParseNamedSize(namedSize);

            // Assert
            Assert.Equal(expectedSize, result);
        }

        [Theory]
        [InlineData("1d", 86400000)]
        [InlineData("1h", 3600000)]
        [InlineData("1m", 60000)]
        [InlineData("1s", 1000)]
        [InlineData("1ms", 1)]
        [InlineData("1000ms", 1000)]
        public void ParseNamedDelay_Should_ParseNamedDelay(string namedDelay, int expectedDelay)
        {
            // Act
            var result = TalaryonHelper.ParseNamedDelay(namedDelay);

            // Assert
            Assert.Equal(TimeSpan.FromMilliseconds(expectedDelay), result);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseBasicConnectionString()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("redis://localhost:6379");

            // Assert
            Assert.Equal("redis", result.Type);
            Assert.Null(result.Username);
            Assert.Null(result.Password);
            Assert.Equal("localhost", result.Host);
            Assert.Equal(6379, result.Port);
            Assert.Null(result.Endpoint);
            Assert.Empty(result.Options);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseWithAuth()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("mysql://user:pass@localhost:3306");

            // Assert
            Assert.Equal("mysql", result.Type);
            Assert.Equal("user", result.Username);
            Assert.Equal("pass", result.Password);
            Assert.Equal("localhost", result.Host);
            Assert.Equal(3306, result.Port);
            Assert.Null(result.Endpoint);
            Assert.Empty(result.Options);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseWithEndpoint()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("http://api.example.com/v1");

            // Assert
            Assert.Equal("http", result.Type);
            Assert.Null(result.Username);
            Assert.Null(result.Password);
            Assert.Equal("api.example.com", result.Host);
            Assert.Null(result.Port);
            Assert.Equal("v1", result.Endpoint);
            Assert.Empty(result.Options);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseWithOptions()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("http://api.example.com?param=value&param2=value2");

            // Assert
            Assert.Equal("http", result.Type);
            Assert.Equal("api.example.com", result.Host);
            Assert.Null(result.Port);
            Assert.Null(result.Endpoint);
            Assert.Equal(2, result.Options.Count);
            Assert.Equal("value", result.Options["param"]);
            Assert.Equal("value2", result.Options["param2"]);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseFullConnectionString()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("ws://user:secret@localhost:8080/chat?token=abc123&reconnect=true");

            // Assert
            Assert.Equal("ws", result.Type);
            Assert.Equal("user", result.Username);
            Assert.Equal("secret", result.Password);
            Assert.Equal("localhost", result.Host);
            Assert.Equal(8080, result.Port);
            Assert.Equal("chat", result.Endpoint);
            Assert.Equal(2, result.Options.Count);
            Assert.Equal("abc123", result.Options["token"]);
            Assert.Equal("true", result.Options["reconnect"]);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseUsernameWithoutPassword()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("https://user@host:8080/api?param=value");

            // Assert
            Assert.Equal("https", result.Type);
            Assert.Equal("user", result.Username);
            Assert.Null(result.Password);
            Assert.Equal("host", result.Host);
            Assert.Equal(8080, result.Port);
            Assert.Equal("api", result.Endpoint);
            Assert.Single(result.Options);
            Assert.Equal("value", result.Options["param"]);
        }

        [Fact]
        public void ParseConnectionString_Should_ParseHostWithoutPort()
        {
            // Act
            var result = TalaryonHelper.ParseConnectionString("type://host");

            // Assert
            Assert.Equal("type", result.Type);
            Assert.Equal("host", result.Host);
            Assert.Null(result.Port);
        }

        [Fact]
        public void ParseConnectionString_Should_ThrowOnEmptyString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => TalaryonHelper.ParseConnectionString(""));
            Assert.Throws<ArgumentException>(() => TalaryonHelper.ParseConnectionString(null!));
        }

        [Fact]
        public void ParseConnectionString_Should_ThrowOnMissingScheme()
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => TalaryonHelper.ParseConnectionString("localhost:6379"));
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}