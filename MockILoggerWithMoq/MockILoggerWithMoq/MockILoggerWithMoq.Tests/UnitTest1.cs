using Microsoft.Extensions.Logging;
using Moq;

namespace MockILoggerWithMoq.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void When_ObjectIsConstructed_Expect_NoErrorsLogged()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>(MockBehavior.Strict);
        loggerMock.Setup(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )
        );

        // Act
        var sut = new SystemUnderTest(loggerMock.Object);

        // Assert
        // We can inspect invocations and get logged messages
        var loggedErrorMessages = loggerMock.Invocations
            .Where(x => (LogLevel)x.Arguments[0] == LogLevel.Error)
            .Select(x => x.Arguments[2].ToString());

        Console.WriteLine("Captured error messages:");
        foreach (var errorMessage in loggedErrorMessages)
        {
            Console.WriteLine($"- {errorMessage}");
        }
        

        // ... or verify that a method was/was not called (this assertion will fail intentionally)
        loggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never,
            "Expected no errors would be written to ILogger, but found some"
        );
    }
}