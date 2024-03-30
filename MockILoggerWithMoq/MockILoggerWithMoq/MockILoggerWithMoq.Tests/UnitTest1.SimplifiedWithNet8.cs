using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace MockILoggerWithMoq.Tests;


[TestClass]
public class Tests // Assuming NUnit is used for unit testing
{
    [TestMethod]
    public void When_ObjectIsConstructed_Expect_NoErrorsLogged()
    {
        // Arrange
        var logger = new FakeLogger<SystemUnderTest>();

        // Act
        var sut = new SystemUnderTest(logger);

        // Assert
        Assert.IsFalse(logger.Collector.GetSnapshot().Any(e => e.Level == LogLevel.Error),
            "Expected no errors would be written to ILogger, but found some.");
    }
}