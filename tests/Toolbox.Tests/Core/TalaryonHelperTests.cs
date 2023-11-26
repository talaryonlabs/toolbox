namespace TalaryonLabs.Toolbox.Tests
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
        public void UUID_Should_ReturnUUID()
        {
            // Act
            var result = TalaryonHelper.UUID();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<string>(result);
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

        private class TestObject
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}